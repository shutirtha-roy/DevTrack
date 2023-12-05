using Autofac;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Extensions;
using DevTrack.Web.Areas.App.Models;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILifetimeScope scope, ILogger<SettingsController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            SettingsEditModel settingsEditModel = _scope.Resolve<SettingsEditModel>();
            var data = await settingsEditModel.GetProfileData();
            return View(data);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Edit(SettingsEditModel settingsEditModel)
        {
            if (ModelState.IsValid)
            {
                settingsEditModel.ResolveDependency(_scope);

                try
                {
                    await settingsEditModel.EditSettings();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully edited your profile details.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in editing profile settings.");

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in changing your profile details.",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(settingsEditModel);
        }

        public async Task<IActionResult> ChangePassword()
        {
            var model = _scope.Resolve<PasswordEditModel>();
            return View(model);
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordEditModel passwordEditModel)
        {
            if (ModelState.IsValid)
            {
                passwordEditModel.ResolveDependency(_scope);

                try
                {
                    await passwordEditModel.ChangePassword();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "You have successfully changed your password.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in changing password.");

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ex.Message,
                        Type = ResponseTypes.Danger
                    });
                }
            }
            
            return View(passwordEditModel);
        }
    }
}