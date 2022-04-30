using Amazon.S3;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Files;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : BaseApiController
    {
        private IFilesService _service = null;
        private IAuthenticationService<int> _authService = null;

        public FilesApiController(IFilesService service
            , ILogger<FilesApiController> logger
            , IAuthenticationService<int> authService, IWebHostEnvironment environment) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<File>> Get(int id)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                File file = _service.Get(id);
                if (file == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("File not found");
                }
                else
                {
                    response = new ItemResponse<File> { Item = file };
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

        [HttpGet]
        public ActionResult<ItemResponse<Paged<File>>> GetAll(int pageIndex, int pageSize)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Paged<File> page = _service.GetAll(pageIndex, pageSize);
                if (page == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<File>> { Item = page };

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

        [HttpGet("user/{userId:int}")]
        public ActionResult<ItemResponse<Paged<File>>> GetAllCreatedBy(int pageIndex, int pageSize, int userId)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
                Paged<File> page = _service.GetCreatedBy(pageIndex, pageSize, userId);
                if (page == null)
                {
                    responseCode = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<File>> { Item = page };
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
        [HttpPost]
        public ActionResult Create(FileAddRequest model)
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

        [HttpPost("upload")]
        public async Task<ActionResult<ItemsResponse<Upload>>> Upload(List<IFormFile> file)
        {
            int responseCode = 200;
            BaseResponse response = null;
                try
                {
                int createdBy = _authService.GetCurrentUserId();
                List<Upload> uploads = await _service.FileUpload(file, createdBy);
                    response = new ItemsResponse<Upload> { Items = uploads };           
                }
                catch (AmazonS3Exception ae)
                {
                responseCode = 500;
                response = new ErrorResponse(ae.Message);
                base.Logger.LogError(ae.ToString());
                }
                catch (Exception ex)
                {
                responseCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(responseCode, response);
        }

        [HttpDelete("{id:int}/{keyName}")]
        public async Task<ActionResult<SuccessResponse>> DeleteFile(int id, string keyName)
        {
            int responseCode = 200;
            BaseResponse response = null;
            try
            {
               await _service.DeleteFile(keyName);
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (AmazonS3Exception ae)
            {
                responseCode = 500;
                response = new ErrorResponse(ae.Message);
                base.Logger.LogError(ae.ToString());
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
        public ActionResult Update(FileUpdateRequest model, int id)
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
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(responseCode, response);
        }     
    }

}
