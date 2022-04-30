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
    public class SurveyQuestionsService : ISurveyQuestionsService
    {
        IDataProvider _data = null;

        public SurveyQuestionsService(IDataProvider data)
        {
            _data = data;
        }

        public SurveyQuestion Get(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Select_ById]";

            SurveyQuestion question = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection fileId)
            {
                fileId.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                question = QuestionMapper(reader, out int startingIndex);
            });
            return question;

        }

        public Paged<SurveyQuestion> GetAll(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[SurveyQuestions_SelectAll]";
            Paged<SurveyQuestion> pagedList = null;
            List<SurveyQuestion> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                SurveyQuestion newQuestion = QuestionMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyQuestion>();
                }
                list.Add(newQuestion);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyQuestion>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<SurveyQuestion> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[SurveyQuestions_Select_ByCreatedBy]";
            Paged<SurveyQuestion> pagedList = null;
            List<SurveyQuestion> list = null;
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
                SurveyQuestion newQuestion = QuestionMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyQuestion>();
                }
                list.Add(newQuestion);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyQuestion>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(SurveyQuestionAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[SurveyQuestions_Insert]";

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

        public void Update(SurveyQuestionUpdateRequest model)
        {
            string procName = "[dbo].[SurveyQuestions_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);
                collection.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void CommonParams(SurveyQuestionAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@UserId", model.UserId);
            collection.AddWithValue("@Question", model.Question);
            collection.AddWithValue("@HelpText", model.HelpText);
            collection.AddWithValue("@IsRequired", model.IsRequired);
            collection.AddWithValue("@IsMultipleAllowed", model.IsMultipleAllowed);
            collection.AddWithValue("@QuestionTypeId", model.QuestionTypeId);
            collection.AddWithValue("@SectionId", model.SectionId);
            collection.AddWithValue("@StatusId", model.StatusId);
            collection.AddWithValue("@SortOrder", model.SortOrder);
        }

        private static SurveyQuestion QuestionMapper(IDataReader reader, out int startingIndex)
        {
            SurveyQuestion question = new SurveyQuestion();
            startingIndex = 0;

            question.Id = reader.GetSafeInt32(startingIndex++);
            question.UserId = reader.GetSafeInt32(startingIndex++);
            question.Question = reader.GetSafeString(startingIndex++);
            question.HelpText = reader.GetSafeString(startingIndex++);
            question.IsRequired = reader.GetSafeBool(startingIndex++);
            question.IsMultipleAllowed = reader.GetSafeBool(startingIndex++);
            question.QuestionType = reader.GetSafeString(startingIndex++);
            question.SectionId = reader.GetSafeInt32(startingIndex++);
            question.Status = reader.GetSafeString(startingIndex++);
            question.SortOrder = reader.GetSafeInt32(startingIndex++);
            question.DateCreated = reader.GetSafeDateTime(startingIndex++);
            question.DateModified = reader.GetSafeDateTime(startingIndex++);

            return question;
        }

    }
}
