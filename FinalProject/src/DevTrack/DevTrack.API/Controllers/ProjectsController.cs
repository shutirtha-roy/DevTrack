using Autofac;
using DevTrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevTrack.API.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("AllowSites")]
    [ApiController]
    [Authorize(Policy = "ApiProjectListRequirementPolicy")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly ILifetimeScope _scope;

        public ProjectsController(ILogger<ProjectsController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpGet]
        public async Task<ProjectResponseModel> Get()
        {
            var projectListModel = _scope.Resolve<ProjectListModel>();
            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
            ProjectResponseModel response = await projectListModel.GetProjectsByUserIdAndWorker(id);

            return response;
        }
    }
}