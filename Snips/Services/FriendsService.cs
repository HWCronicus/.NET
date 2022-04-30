using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests.Friends;
using Sabio.Models;
using Sabio.Models.Requests.Skills;

namespace Sabio.Services
{
    public class FriendsService : IFriendsService
    {
        IDataProvider _data = null;

        public FriendsService(IDataProvider data)
        {
            _data = data;

        }

        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV3]";

            Friend friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection friendId)
            {
                friendId.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                friend = FriendMapper(reader, out int startingIndex);

            });
            return friend;

        }

        public List<Friend> GetAll()
        {
            string procName = "[dbo].[Friends_SelectAll_v3]";

            List<Friend> friends = null;

            _data.ExecuteCmd(procName, null, delegate (IDataReader reader, short set)
            {
                Friend newFriend = FriendMapper(reader, out int startingIndex);
                if (friends == null)
                {
                    friends = new List<Friend>();
                }
                friends.Add(newFriend);
            });
            return friends;
        }

        public Paged<Friend> GetPaginate(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Friends_SelectPaginated_v3]";
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , (collection) =>
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
            }
            , (reader, recordSetIndex) =>
            {
                Friend newFriend = FriendMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<Friend>();
                }
                list.Add(newFriend);
            });
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<Friend> Search(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Friends_SearchPaginated_v3]";
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName
            , delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@pageIndex", pageIndex);
                collection.AddWithValue("@pageSize", pageSize);
                collection.AddWithValue("@Query", query);
            }
            , delegate (IDataReader reader, short set)
            {
                Friend newFriend = FriendMapper(reader, out int startingIndex);
                totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<Friend>();
                }
                list.Add(newFriend);
            });
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public int Add(FriendAddRequest model)
        {
            int Id = 0;
            string procName = "[dbo].[Friends_Insert_v4]";
            DataTable skills = MapSkillsToTable(model.Skills);

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonInputs(model, collection, skills);
               
                SqlParameter idOut = new SqlParameter("@FriendId", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                collection.Add(idOut);

            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object idOut = returnCollection["@FriendId"].Value;
                int.TryParse(idOut.ToString(), out Id);
            });

            return Id;
        }

        public void Update(FriendUpdateRequest model)
        {
            string procName = "[dbo].[Friends_UpdateV2]";
            DataTable skills = MapSkillsToTable(model.Skills);

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                CommonInputs(model, collection, skills);
                collection.AddWithValue("@PrimaryImageId", model.PrimaryImageId);
                collection.AddWithValue("@id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Friends_DeleteV2]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void CommonInputs(FriendAddRequest model, SqlParameterCollection collection, DataTable skills)
        {
            collection.AddWithValue("@Title", model.Title);
            collection.AddWithValue("@Bio", model.Bio);
            collection.AddWithValue("@Summary", model.Summary);
            collection.AddWithValue("@Headline", model.Headline);
            collection.AddWithValue("@Slug", model.Slug);
            collection.AddWithValue("@StatusId", model.StatusId);
            collection.AddWithValue("@Url", model.Url);
            collection.AddWithValue("@typeId", model.TypeId);
            collection.AddWithValue("@Skills", skills);
        }

        private static Friend FriendMapper(IDataReader reader, out int startingIndex)
        {
            Friend friend = new Friend();
            startingIndex = 0;


            friend.Id = reader.GetSafeInt32(startingIndex++);
            friend.Title = reader.GetSafeString(startingIndex++);
            friend.Bio = reader.GetSafeString(startingIndex++);
            friend.Summary = reader.GetSafeString(startingIndex++);
            friend.Headline = reader.GetSafeString(startingIndex++);
            friend.Slug = reader.GetSafeString(startingIndex++);
            friend.StatusId = reader.GetSafeInt32(startingIndex++);
            friend.PrimaryImageId = reader.GetSafeInt32(startingIndex++);
            friend.Url = reader.GetSafeString(startingIndex++);
            friend.TypeId = reader.GetSafeInt32(startingIndex++);   
            friend.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++); 
            friend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            friend.DateModified = reader.GetSafeDateTime(startingIndex++);
            return friend;

        }

        private DataTable MapSkillsToTable(List<string> skillsToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            foreach (var singleSkill in skillsToMap)
            {
                DataRow dr = dt.NewRow();  
                int startingIndex = 0;
                dr.SetField(startingIndex, singleSkill);
                dt.Rows.Add(dr);
            }
            return dt;  
        }
    }
}
