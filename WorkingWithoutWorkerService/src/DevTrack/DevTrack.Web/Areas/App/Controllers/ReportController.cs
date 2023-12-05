using Autofac;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Extensions;
using DevTrack.Web.Areas.App.Models;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App"), Authorize]
    public class ReportController : BaseController<ReportController>
    {
        public ReportController(ILifetimeScope scope, ILogger<ReportController> reportLogger) : base(scope, reportLogger)
        {
            
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> MyReport()
        {
            var model = _scope.Resolve<ActivityProjectUserModel>();
            model.WorkerUserId = await model.GetLoggedInUserId();
            await model.SetUserProjectsWhenWorker(model.WorkerUserId);
            return View(model);
        }


        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> MyReport(ActivityProjectUserModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);

                ActivityRequestModel activityModel = _scope.Resolve<ActivityRequestModel>();

                try
                {
                    model.ReportActivityData = await activityModel.GetActivitiesReport(model.WorkerUserId, model.ProjectId, model.StartTime, model.EndTime);
                    await model.SetUserProjectsWhenWorker(model.WorkerUserId);
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No Reports Were Found");

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "No Reports Were Found",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        public async Task<IActionResult> OwnerReport()
        {
            var model = _scope.Resolve<ActivityProjectUserModel>();
            model.OwnerUserId = await model.GetLoggedInUserId();
            await model.SetUserProjectsWhenOwner(model.OwnerUserId);
            return View(model);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> OwnerReport(ActivityProjectUserModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);

                ActivityRequestModel activityModel = _scope.Resolve<ActivityRequestModel>();

                try
                {
                    model.ReportActivityData = await activityModel.GetActivitiesReport(model.WorkerUserId, model.ProjectId, model.StartTime, model.EndTime);
                    await model.SetUserProjectsWhenOwner(model.OwnerUserId);
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No Reports Were Found");

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "No Reports Were Found",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberList(Guid projectId)
        {
            var model = _scope.Resolve<ActivityProjectUserModel>();
            await model.SetProjectUsers(projectId);

            return Json(model.UserList);
        }
    }
}