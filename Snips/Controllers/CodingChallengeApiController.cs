using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.CodeingChallenge;
using Sabio.Models.Requests.CodeingChallenge;
using Sabio.Models.Requests.CodingChallenge;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers.CodeingChallenge
{
    [Route("api/CodingChallenge")]
    [ApiController]
    public class CodingChallengeApiController : BaseApiController
    {
        private ICodingChallengeService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CodingChallengeApiController(ICodingChallengeService service
          , ILogger<CodingChallengeApiController> logger
          , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpPost]
        public ActionResult Create(CourseAddRequest model)
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Course>> GetById(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Course course = _service.GetById(id);
                if (course == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("User not found");
                }
                else
                {
                    response = new ItemResponse<Course> { Item = course };
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

        [HttpPut("{id:int}")]
        public ActionResult Update(CourseUpdateRequest model, int id)
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

        [HttpGet]
        public ActionResult<ItemResponse<Paged<Course>>> CoursesDetails(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Course> paged = _service.CoursesDetails(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Course>> response = new ItemResponse<Paged<Course>>() { Item = paged };

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
    }
}
