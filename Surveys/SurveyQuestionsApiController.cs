using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Survey;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers.SurveyApiControllers
{
    [Route("api/survey/question")]
    [ApiController]
    public class SurveyQuestionsApiController : BaseApiController
    {
        private ISurveyQuestionsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public SurveyQuestionsApiController(ISurveyQuestionsService service
            , ILogger<SurveyQuestionsApiController> logger
            , IAuthenticationService<int> authService, IWebHostEnvironment environment) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<SurveyQuestion>> Get(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                SurveyQuestion file = _service.Get(id);
                if (file == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("File not found");
                }
                else
                {
                    response = new ItemResponse<SurveyQuestion> { Item = file };
                }
            }
            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse($" Generic Error {ex.Message}");
                Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<SurveyQuestion>>> GetAll(int pageIndex, int pageSize)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyQuestion> page = _service.GetAll(pageIndex, pageSize);
                if (page == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyQuestion>> { Item = page };

                }
            }
            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());


            }
            return StatusCode(responseCode, response);
        }

        [HttpGet("user/{userId:int}")]
        public ActionResult<ItemResponse<Paged<SurveyQuestion>>> GetAllCreatedBy(int pageIndex, int pageSize, int userId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyQuestion> page = _service.GetCreatedBy(pageIndex, pageSize, userId);
                if (page == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyQuestion>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                responseCode = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(responseCode, response);
        }
        [HttpPost]
        public ActionResult Create(SurveyQuestionAddRequest model)
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
        public ActionResult Update(SurveyQuestionUpdateRequest model, int id)
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
                    result = StatusCode(404, errorResponse);
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
                Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }

    }
}
