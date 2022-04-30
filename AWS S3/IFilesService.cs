using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IFilesService
    {
        Task<List<Upload>> FileUpload(List<IFormFile> files, int userId);
        Task<DeleteObjectResponse> DeleteFile(string keyName);
        File Get(int id);
        Paged<File> GetAll(int pageIndex, int pageSize);
        Paged<File> GetCreatedBy(int pageIndex, int pageSize, int userId);
        int Add(FileAddRequest model);
        void Update(FileUpdateRequest model);
        void Delete(int id);
    }
}
