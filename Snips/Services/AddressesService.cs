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
using Sabio.Models.Requests.Addresses;

namespace Sabio.Services
{
    public class AddressesService : IAddressesService
    {
        IDataProvider _data = null;

        public AddressesService(IDataProvider data)
        {
            _data = data;

        }
        public int Add(AddressAddRequest model)
        {
            int Id = 0;

            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@LineOne", model.LineOne);
                collection.AddWithValue("@SuiteNumber", model.SuiteNumber);
                collection.AddWithValue("@City", model.City);
                collection.AddWithValue("@State", model.State);
                collection.AddWithValue("@PostalCode", model.PostalCode);
                collection.AddWithValue("@IsActive", model.IsActive);
                collection.AddWithValue("@Lat", model.Lat);
                collection.AddWithValue("@Long", model.Long);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                collection.Add(idOut);

            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object outId = returnCollection["@Id"].Value;
                Int32.TryParse(outId.ToString(), out Id);

            });

            return Id;
        }

        public void Update(AddressUpdateRequest model)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@LineOne", model.LineOne);
                collection.AddWithValue("@SuiteNumber", model.SuiteNumber);
                collection.AddWithValue("@City", model.City);
                collection.AddWithValue("@State", model.State);
                collection.AddWithValue("@PostalCode", model.PostalCode);
                collection.AddWithValue("@IsActive", model.IsActive);
                collection.AddWithValue("@Lat", model.Lat);
                collection.AddWithValue("@Long", model.Long);
                collection.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public Address Get(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                address = MapAddress(reader);
            }
            );

            return address;
        }

        public List<Address> GetTop()
        {
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            List<Address> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Address newAddress = MapAddress(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }

                list.Add(newAddress);
            }
);

            return list;
        }

        public List<Address> GetAll()
        {
            string procName = "[dbo].[Sabio_Addresses_SelectAll]";

            List<Address> allAddress = null;

            _data.ExecuteCmd(procName, null, delegate (IDataReader reader, short set)
            {
                Address newAddress = MapAddress(reader);
                if (allAddress == null)
                {
                    allAddress = new List<Address>();
                }

                allAddress.Add(newAddress);
            }
            );

            return allAddress;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection idCollection)
            {
                idCollection.AddWithValue("@Id", id);

            }, returnParameters: null);

        }

        private static Address MapAddress(IDataReader reader)
        {
            Address newAddress = new Address();

            int startingIndex = 0;

            newAddress.Id = reader.GetSafeInt32(startingIndex++);
            newAddress.LineOne = reader.GetSafeString(startingIndex++);
            newAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            newAddress.City = reader.GetSafeString(startingIndex++);
            newAddress.State = reader.GetSafeString(startingIndex++);
            newAddress.PostalCode = reader.GetSafeString(startingIndex++);
            newAddress.IsActive = reader.GetSafeBool(startingIndex++);
            newAddress.Lat = reader.GetSafeDouble(startingIndex++);
            newAddress.Long = reader.GetSafeDouble(startingIndex++);
            return newAddress;
        }
    }
}
