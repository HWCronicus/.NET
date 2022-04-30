using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Survey;
using Sabio.Models.Requests.Surveys;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class SurveyQuestionAnswerOptionsService : ISurveyQuestionAnswerOptionsService
    {
        IDataProvider _data = null;

        public SurveyQuestionAnswerOptionsService(IDataProvider data)
        {
            _data = data;
        }

        public SurveyQuestionAnswerOption Get(int id)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOption_Select_ById]";

            SurveyQuestionAnswerOption answerOption = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection fileId)
            {
                fileId.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                answerOption = SurveyQuestionAnswerOptionMapper(reader, out int startingIndex);
            });
            return answerOption;

        }

        public Paged<SurveyQuestionAnswerOption> GetAll(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOption_SelectAll]";
            Paged<SurveyQuestionAnswerOption> pagedList = null;
            List<SurveyQuestionAnswerOption> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                SurveyQuestionAnswerOption newAnswerOption = SurveyQuestionAnswerOptionMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyQuestionAnswerOption>();
                }
                list.Add(newAnswerOption);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyQuestionAnswerOption>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<SurveyQuestionAnswerOption> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOption_Select_ByCreatedBy]";
            Paged<SurveyQuestionAnswerOption> pagedList = null;
            List<SurveyQuestionAnswerOption> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@UserId", userId);
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                SurveyQuestionAnswerOption newAnswerOption = SurveyQuestionAnswerOptionMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyQuestionAnswerOption>();
                }
                list.Add(newAnswerOption);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyQuestionAnswerOption>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(SurveyQuestionAnswerOptionAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[SurveyQuestionAnswerOption_Insert]";

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

        public void Update(SurveyQuestionAnswerOptionUpdateRequest model)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOption_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);
                collection.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyQuestionAnswerOption_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void CommonParams(SurveyQuestionAnswerOptionAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@QuestionId", model.QuestionId);
            collection.AddWithValue("@Text", model.Text);
            collection.AddWithValue("@Value", model.Value);
            collection.AddWithValue("@AdditionalInfo", model.AdditionalInfo);
            collection.AddWithValue("@CreatedBy", model.CreatedBy);
        }

        private static SurveyQuestionAnswerOption SurveyQuestionAnswerOptionMapper(IDataReader reader, out int startingIndex)
        {
            SurveyQuestionAnswerOption answerOption = new SurveyQuestionAnswerOption();
            startingIndex = 0;

            answerOption.Id = reader.GetSafeInt32(startingIndex++);
            answerOption.QuestionId = reader.GetSafeInt32(startingIndex++);
            answerOption.Text = reader.GetSafeString(startingIndex++);
            answerOption.Value = reader.GetSafeString(startingIndex++);
            answerOption.AdditionalInfo = reader.GetSafeString(startingIndex++);
            answerOption.CreatedBy = reader.GetSafeInt32(startingIndex++);
            answerOption.DateCreated = reader.GetSafeDateTime(startingIndex++);
            answerOption.DateModified = reader.GetSafeDateTime(startingIndex++);

            return answerOption;
        }
    }
}
