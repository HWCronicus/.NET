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
    public class SurveyAnswersService : ISurveyAnswersService
    {
        IDataProvider _data = null;

        public SurveyAnswersService(IDataProvider data)
        {
            _data = data;
        }

        public SurveyAnswer Get(int id)
        {
            string procName = "[dbo].[SurveyAnswers_Select_ById]";

            SurveyAnswer answer = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection fileId)
            {
                fileId.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                answer = AnswerMapper(reader, out int startingIndex);
            });
            return answer;

        }

        public Paged<SurveyAnswer> GetAll(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[SurveyAnswers_SelectAll]";
            Paged<SurveyAnswer> pagedList = null;
            List<SurveyAnswer> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                SurveyAnswer newAnswer = AnswerMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyAnswer>();
                }
                list.Add(newAnswer);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyAnswer>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<SurveyAnswer> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[SurveyAnswers_Select_ByCreatedBy]";
            Paged<SurveyAnswer> pagedList = null;
            List<SurveyAnswer> list = null;
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
                SurveyAnswer newAnswer = AnswerMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<SurveyAnswer>();
                }
                list.Add(newAnswer);
            });
            if (list != null)
            {
                pagedList = new Paged<SurveyAnswer>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(SurveyAnswerAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[SurveyAnswers_Insert]";

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

        public void Update(SurveyAnswerUpdateRequest model)
        {
            string procName = "[dbo].[SurveyAnswers_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);
                collection.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyAnswers_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void CommonParams(SurveyAnswerAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@InstanceId", model.InstanceId);
            collection.AddWithValue("@QuestionId", model.QuestionId);
            collection.AddWithValue("@AnswerOptionId", model.AnswerOptionId);
            collection.AddWithValue("@Answer", model.Answer);
            collection.AddWithValue("@AnswerNumber", model.AnswerNumber);
        }

        private static SurveyAnswer AnswerMapper(IDataReader reader, out int startingIndex)
        {
            SurveyAnswer answer = new SurveyAnswer();
            startingIndex = 0;

            answer.Id = reader.GetSafeInt32(startingIndex++);
            answer.InstanceId = reader.GetSafeInt32(startingIndex++);
            answer.QuestionId = reader.GetSafeInt32(startingIndex++);
            answer.AnswerOptionId = reader.GetSafeInt32(startingIndex++);
            answer.Answer = reader.GetSafeString(startingIndex++);
            answer.AnswerNumber = reader.GetSafeInt32(startingIndex++);
            answer.DateCreated = reader.GetSafeDateTime(startingIndex++);
            answer.DateModified = reader.GetSafeDateTime(startingIndex++);

            return answer;
        }
    }
}
