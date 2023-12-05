using Autofac;
using DevTrack.Web.Areas.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App"), Authorize]
    public class HomeController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProjectController> _logger;

        public HomeController(ILifetimeScope scope, ILogger<ProjectController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<object>> GetBarChartData()
        {
            var userProjectModel = _scope.Resolve<UserProjectsModel>();
            await userProjectModel.GetLoggedHoursPerProject();

            var data = new List<object>
            {
                userProjectModel.OwnerProjectTitles,
                userProjectModel.WorkerProjectTitles,
                userProjectModel.LogHoursPerProjectOwner,
                userProjectModel.LogHoursPerProjectWorker,
                userProjectModel.LogHoursPerMonthOwner,
                userProjectModel.LogHoursPerMonthWorker
            };

            return data;
        }

        public IActionResult Blank()
        {
            return View();
        }
    }
}