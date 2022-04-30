using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IAddressesService
    {
        int Add(AddressAddRequest model);
        void Delete(int id);
        Address Get(int id);
        List<Address> GetAll();
        List<Address> GetTop();
        void Update(AddressUpdateRequest model);
    }
}