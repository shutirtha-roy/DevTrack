using Autofac;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Extensions;
using DevTrack.Web.Areas.Admin.Models;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(ILifetimeScope scope, ILogger<HomeController> homeLogger) : base(scope, homeLogger)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = _scope.Resolve<UserModel>();

            try
            {
                await model.SetData(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We are unable to fetch user data.");

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "We are unable to fetch user data.",
                    Type = ResponseTypes.Danger
                });
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(Guid id)
        {
            var model = _scope.Resolve<UserClaimsViewModel>();
            await model.SetData(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            model.ResolveDependency(_scope);

            await model.UpdateExistingUserClaims(model.UserId);
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetUserDetails(Guid userId)
        {
            var model = _scope.Resolve<UserModel>();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersData()
        {
            var model = _scope.Resolve<UserModel>();

            var allUsers = await model.GetAllApplicationUsers();

            return Json(new { data = allUsers });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectOwnerData(Guid id)
        {
            var model = _scope.Resolve<UserModel>();

            var allOwnerProjects = await model.GetUserOwnerProjects(id);

            return Json(new { data = allOwnerProjects });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectWorkerData(Guid id)
        {
            var model = _scope.Resolve<UserModel>();

            var allWorkerProjects = await model.GetUserWorkerProjects(id);

            return Json(new { data = allWorkerProjects });
        }
    }
}