using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
    {
        private IFriendsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FriendApiController(IFriendsService service
            , ILogger<FriendApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("All")]
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                List<Friend> list = _service.GetAll();

                if (list == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("There are currently no Users in the Database");

                }
                else
                {
                    response = new ItemsResponse<Friend> { Items = list };
                }

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
        public ActionResult<ItemResponse<Friend>> Get(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Friend friend = _service.Get(id);
                if (friend == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("User not found");
                }
                else
                {
                    response = new ItemResponse<Friend> { Item = friend };
                }
            }
            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse($" Generic Error {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemsResponse<Friend>> Search(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;
            try
            {
                Paged<Friend> paged = _service.Search(pageIndex, pageSize, query);

                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Friend>> response = new ItemResponse<Paged<Friend>>() { Item = paged };
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, errorResponse);
            }
            return result;
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<Friend>>> Pagination(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Friend> paged = _service.GetPaginate(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                } else
                {
                    ItemResponse<Paged<Friend>> response = new ItemResponse<Paged<Friend>>() { Item = paged };
                    
                    result = Ok200(response);   
                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, errorResponse);

            }
            return result;
        }

        [HttpPost]
        public ActionResult Create(FriendAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int id = _service.Add(model);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };
                result = Created201(response);
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
        public ActionResult Update(FriendUpdateRequest model, int id)
        {
            ObjectResult result = null;
            try
            {
                if (id == model.Id)
                {
                    _service.Update(model);
                    SuccessResponse response = new SuccessResponse();
                    result = StatusCode(201, response);
                }
                else
                {
                    ErrorResponse errorResponse = new ErrorResponse("Id does not match");
                    result = StatusCode(420, errorResponse);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse errorResponse = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, errorResponse);

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
