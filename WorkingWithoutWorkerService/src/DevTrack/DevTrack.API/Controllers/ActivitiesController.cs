using Autofac;
using DevTrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTrack.API.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("AllowSites")]
    [ApiController]
    [Authorize(Policy = "ApiProjectListRequirementPolicy")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly ILifetimeScope _scope;

        public ActivitiesController(ILogger<ActivitiesController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpPost]
        public async Task<ActivityResponseModel> Post(ActivityRequestModel activityRequest)
        {
            var responseModel = new ActivityResponseModel();

            if (ModelState.IsValid)
            {
                try
                {
                    activityRequest.ResolveDependency(_scope);
                    await activityRequest.CreateActivity();

                    responseModel.IsSuccess = true;
                    responseModel.StatusCode = (int)HttpStatusCode.OK;
                    responseModel.Data = null;
                    responseModel.Errors = new string[] {};
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);

                    responseModel.IsSuccess = false;
                    responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Data = null;
                    responseModel.Errors = new string[] { e.Message };
                }
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Data = null;
                responseModel.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
            }

            return responseModel;
        }
    }
}