using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.AppSettings;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Files;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class FilesService : IFilesService
    {
        IDataProvider _data = null;
        private AWSStorageConfig _awsConfig = null;

        public FilesService(IDataProvider data, IOptions<AWSStorageConfig> awsConfig) 
        { 
            _data = data;
            _awsConfig = awsConfig.Value;
        }

        public async Task<List<Upload>> FileUpload(List<IFormFile> files, int userId)
        {
            List<Upload> uploads = new List<Upload>();
            foreach (IFormFile file in files)
            {
                Upload upload = new Upload();

                string keyName = Guid.NewGuid() + "_" + file.FileName;
                string url = _awsConfig.Domain + keyName;
                string bucketName = _awsConfig.BucketName;
                string extension = System.IO.Path.GetExtension(file.FileName);
                int Id = 0;
                string procName = "[dbo].[Files_Upload]";

                using (var client = new AmazonS3Client(_awsConfig.AccessKey, _awsConfig.Secret, RegionEndpoint.GetBySystemName(_awsConfig.BucketRegion)))
                {
                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(file.OpenReadStream(), bucketName, keyName);
                }

                _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection collection)
                {
                    collection.AddWithValue("@Url", url);
                    collection.AddWithValue("@FileType", extension);
                    collection.AddWithValue("@CreatedBy", userId);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    collection.Add(idOut);

                },
                delegate (SqlParameterCollection returnCollection)
                {
                    object idOut = returnCollection["@Id"].Value;
                    int.TryParse(idOut.ToString(), out Id);
                });

                upload.Url = url;
                upload.Id = Id;
                uploads.Add(upload);
            }
            return uploads;
        }

        public async Task<DeleteObjectResponse> DeleteFile(string keyName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = keyName
            };

            IAmazonS3 client = new AmazonS3Client(_awsConfig.AccessKey, _awsConfig.Secret, RegionEndpoint.GetBySystemName(_awsConfig.BucketRegion));
            DeleteObjectResponse response = await client.DeleteObjectAsync(deleteObjectRequest);
            return response;
        }

        public File Get(int id)
        {
            string procName = "[dbo].[Files_Select_ById]";

            File file = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection fileId)
            {
                fileId.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                file = FileMapper(reader, out int startingIndex);
            });
            return file;

        }

        public Paged<File> GetAll(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Files_SelectAll]";
            Paged<File> pagedList = null;
            List<File> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                File newFile = FileMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<File>();
                }
                list.Add(newFile);
            });
            if (list != null)
            {
                pagedList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<File> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[Files_Select_ByCreatedBy]";
            Paged<File> pagedList = null;
            List<File> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
                collection.AddWithValue("@UserId", userId);
            }
            , (reader, recordSetIndex) =>
            {
                File newFile = FileMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<File>();
                }
                list.Add(newFile);
            });
            if (list != null)
            {
                pagedList = new Paged<File>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(FileAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[Files_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                collection.Add(idOut);

            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object idOut = returnCollection["@Id"].Value;
                int.TryParse(idOut.ToString(), out Id);
            });

            return Id;
        }

        public void Update(FileUpdateRequest model)
        {
            string procName = "[dbo].[Files_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);
                collection.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Files_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void CommonParams(FileAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Url", model.Url);
            collection.AddWithValue("@FileTypeId", model.FileTypeId);
            collection.AddWithValue("@CreatedBy", model.CreatedBy);
        }

        private static File FileMapper(IDataReader reader, out int startingIndex)
        {
            File file = new File();
            startingIndex = 0;

            file.Id = reader.GetSafeInt32(startingIndex++);
            file.Url = reader.GetSafeString(startingIndex++);
            file.FileType = reader.GetSafeString(startingIndex++);
            file.CreatedBy = reader.GetSafeInt32(startingIndex++);
            file.DateCreated = reader.GetSafeDateTime(startingIndex++);
            file.DateModified = reader.GetSafeDateTime(startingIndex++);

            return file;
        }

    }
}
