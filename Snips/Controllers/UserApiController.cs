using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUsersService _service = null;
        private IAuthenticationService<int> _authService;

        public UserApiController(IUsersService service
            , ILogger<UserApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                List<User> list = _service.GetAll();

                if (list == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("There are currently no Users in the Database");

                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> Get(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try { 
                User user = _service.Get(id);
                if (user == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("User not found");
                }
                else
                {
                    response= new ItemResponse<User> { Item = user };
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

        [HttpPost]
        public ActionResult Create(UserAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int id = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };
                result = Created201(response);
            }
            catch (SqlException sqlEx)
            {
                ErrorResponse errorResponse = new ErrorResponse($"SQL Error {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
                result = StatusCode(500, errorResponse);
            }

            catch (ArgumentException argEx)
            {
                ErrorResponse errorResponse = new ErrorResponse($"Argument Error: {argEx.Message}");
                base.Logger.LogError(argEx.ToString());
                result = StatusCode(500, errorResponse);
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, errorResponse);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(UserUpdateRequest model, int id)
        {
            ObjectResult result = null;
            try
            {
                if (id == model.Id)
                {
                    _service.Update(model);
                    SuccessResponse response = new SuccessResponse();
                    result = StatusCode(201, response);
                } else
                {
                    ErrorResponse errorResponse = new ErrorResponse("Id does not match");
                    result = StatusCode(420, errorResponse);
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorResponse errorResponse = new ErrorResponse($"SQL Error {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
                result = StatusCode(500, errorResponse);
            }

            catch (ArgumentException argEx)
            {
                ErrorResponse errorResponse = new ErrorResponse($"Argument Error: {argEx.Message}");
                base.Logger.LogError(argEx.ToString());
                result = StatusCode(500, errorResponse);
            }
            catch (Exception ex) 
            { 
                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result= StatusCode(500, errorResponse);
            
            }
            return result;
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

    }
}
