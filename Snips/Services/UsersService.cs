using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UsersService : IUsersService
    {
        IDataProvider _data = null;

        public UsersService(IDataProvider data)
        {
            _data = data;
        }

        public User Get(int id)
        {
            string procname = "[dbo].[Users_SelectById]";
            User user = null;

            _data.ExecuteCmd(procname, delegate (SqlParameterCollection userId)
            {
                userId.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                user = UserMapper(reader, out int startingIndex);
            });

            return user;
        }

        public List<User> GetAll()
        {
            string procname = "[dbo].[Users_SelectAll]";
            List<User> list = null;

            _data.ExecuteCmd(procname, null, delegate (IDataReader reader, short set)
            {
                User newUser = UserMapper(reader, out int startingIndex);
                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(newUser);
            });

            return list;
        }

        public int Add(UserAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[Users_Insert]";

            _data.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection collection)
                {
                    CommonInput(model, collection);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    collection.Add(idOut);
                },
                delegate (SqlParameterCollection output)
                {
                    object idOut = output["@Id"].Value;
                    int.TryParse(idOut.ToString(), out id);
                });

            return id;
        }

        public void Update(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection collection)
                {
                    CommonInput(model, collection);
                    collection.AddWithValue("@Id", model.Id);
                },
                null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
            }, null);

        }

        private static void CommonInput(UserAddRequest model, SqlParameterCollection collection)
        {
            collection.AddWithValue("@firstName", model.firstName);
            collection.AddWithValue("@lastName", model.lastName);
            collection.AddWithValue("@email", model.email);
            collection.AddWithValue("@password", model.password);
            collection.AddWithValue("@passwordConfirm", model.passwordConfirm);
            collection.AddWithValue("@avatarUrl", model.avatarUrl);
            collection.AddWithValue("@tenantId", model.tenantId);
        }

        private static User UserMapper(IDataReader reader, out int startingIndex)
        {
            User user = new User();
            startingIndex = 0;

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.firstName = reader.GetSafeString(startingIndex++);
            user.lastName = reader.GetSafeString(startingIndex++);
            user.email = reader.GetSafeString(startingIndex++);
            user.password = reader.GetSafeString(startingIndex++);
            user.passwordConfirm = reader.GetSafeString(startingIndex++);
            user.avatarUrl = reader.GetSafeString(startingIndex++);
            user.tenantId = reader.GetSafeString(startingIndex++);
            user.DateAdded = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            return user;

        }

    }
}
