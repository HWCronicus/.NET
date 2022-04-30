using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.CodeingChallenge;
using Sabio.Models.Requests.CodeingChallenge;
using Sabio.Models.Requests.CodingChallenge;
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
    public class CodingChallengeService : ICodingChallengeService
    {
        IDataProvider _data = null;

        public CodingChallengeService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(CourseAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[CodeingChallenge_Courses_Insert]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                collection.Add(idOut);

            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object outId = returnCollection["@Id"].Value;
                int.TryParse(outId.ToString(), out Id);

            });
            return Id;
        }

        public Course GetById(int id)
        {
            string procName = "[dbo].[CodeingChallenge_Courses_Select_ByCourseId]";

            Course course = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection friendId)
            {
                friendId.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                course = CourseMapper(reader, out int startingIndex);

            });
            return course;
        }

        public void Update(CourseUpdateRequest model)
        {
            string procName = "[dbo].[CodeingChallenge_Courses_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonParams(model, collection);
                collection.AddWithValue("@id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[CodeingChallenge_Student_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        public Paged<Course> CoursesDetails(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[CodeingChallenge_Courses_Select_CoursesDetails]";
            Paged<Course> pagedList = null;
            List<Course> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                Course newCourse = CourseMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<Course>();
                }
                list.Add(newCourse);
            });
            if (list != null)
            {
                pagedList = new Paged<Course>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        private static Course CourseMapper(IDataReader reader, out int startingIndex)
        {
            Course course = new Course();
            startingIndex = 0;

            course.Id = reader.GetSafeInt32(startingIndex++);
            course.Name = reader.GetSafeString(startingIndex++);
            course.Description = reader.GetSafeString(startingIndex++);
            course.SeasonTerm = reader.GetSafeString(startingIndex++);
            course.Teacher = reader.GetSafeString(startingIndex++);
            course.Students = reader.DeserializeObject<List<Student>>(startingIndex++);
            return course;
        }
        private static void CommonParams(CourseAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@Name", model.Name);
            collection.AddWithValue("@Description", model.Description);
            collection.AddWithValue("@SeasonTermId", model.SeasonTermId);
            collection.AddWithValue("@TeacherId", model.TeacherId);
        }
    }
}
