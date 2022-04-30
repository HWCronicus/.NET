using Sabio.Models.Domain;
using Sabio.Models.Requests.Users;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IUsersService
    {
        int Add(UserAddRequest model);
        void Delete(int id);
        User Get(int id);
        List<User> GetAll();
        void Update(UserUpdateRequest model);
    }
}