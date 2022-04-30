using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressesService _service = null;
        private IAuthenticationService<int> _authService;
        public AddressApiController(IAddressesService service
            , ILogger<AddressApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            int responseCode = 200;
            BaseResponse response = null;

            try
            {
                List<Address> list = _service.GetTop();

                if (list == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("No Addresses Found in Database");
                }

                else
                {
                    response = new ItemsResponse<Address> { Items = list };
                }
            }

            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(responseCode, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Address address = _service.Get(id);
                if (address == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("Address not found");
                }
                else
                {
                    responseCode = 200;
                    response = new ItemResponse<Address> { Item = address };
                }
            }
            catch (SqlException sqlEx)
            {
                responseCode = 500;
                response = new ErrorResponse($"SQL Error {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
            }

            catch (ArgumentException argEx)
            {
                responseCode = 500;
                response = new ErrorResponse($"Argument Error: {argEx.Message}");
                base.Logger.LogError(argEx.ToString());
            }

            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse($" Generic Error {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }

        [HttpPost]
        public ActionResult Create(AddressAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int>() {  Item = id };
                result = Created201(response);

            }
            catch(Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, response);
            }


            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(AddressUpdateRequest model)
        {
            ObjectResult result = null;
            try
            {
                    _service.Update(model);
                    SuccessResponse response = new SuccessResponse();
                    result = StatusCode(201, response);
 
            }
            catch(Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
                result = StatusCode(500, response);

            }

            return result;
        }


    }
}
