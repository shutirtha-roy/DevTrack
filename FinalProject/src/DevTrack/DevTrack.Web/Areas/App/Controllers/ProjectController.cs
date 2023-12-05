using Autofac;
using DevTrack.Infrastructure.Enum;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.Extensions;
using DevTrack.Web.Areas.App.Models;
using DevTrack.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Security.Authentication;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILifetimeScope scope, ILogger<ProjectController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Projects";

            IList<SelectListItem> RoleDDL = new List<SelectListItem>();

            foreach (int i in Enum.GetValues(typeof(ProjectRole)))
            {
                RoleDDL.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(ProjectRole), i),
                    Value = i.ToString()
                });
            }

            return View(RoleDDL);
        }

        public IActionResult Create()
        {
            ProjectCreateModel model = _scope.Resolve<ProjectCreateModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);

                try
                {
                    await model.CreateProject();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully added a new project.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (DuplicateException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in creating project.",
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = _scope.Resolve<ProjectViewModel>();
            var invitation = await model.GetUserInvitation(id);
            if (invitation != null)
            {
                return RedirectToAction("Invitation", new { id = invitation.Id });
            }
            await model.LoadData(id);

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = _scope.Resolve<ProjectEditModel>();
            await model.LoadData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.ResolveDependency(_scope);

                try
                {
                    await model.EditProject();

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "Successfully edited a new project.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch (AuthenticationException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (DuplicateException ioe)
                {
                    _logger.LogError(ioe, ioe.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = ioe.Message,
                        Type = ResponseTypes.Danger
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                    {
                        Message = "There was a problem in editing project.",
                        Type = ResponseTypes.Danger
                    });
                }
            }
            return View(model);
        }

        public async Task<JsonResult> GetProjectData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            int role = int.Parse(Request.Query["SearchItem[Role]"].FirstOrDefault());
            int status = int.Parse(Request.Query["SearchItem[Status]"].FirstOrDefault());

            var model = _scope.Resolve<ProjectListModel>();
            model.SearchItem = _scope.Resolve<ProjectSearch>();
            model.SearchItem.IsArchived = status > 0;
            model.SearchItem.Role = role > 0 ? role : null;

            return Json(await model.GetProjectsPagedData(dataTableModel));
        }

        public async Task<JsonResult> GetEmails()
        {
            var projectId = Guid.Parse(Request.Query["projectId"].FirstOrDefault());
            var model = _scope.Resolve<ProjectUserEmailListModel>(new TypedParameter(typeof(Guid), projectId));

            return Json(await model.GetUserEmails());
        }

        [HttpPost]
        public async Task GetEmails(List<string> invitationEmails, string projectId)
        {
            foreach (var email in invitationEmails)
            {
                var invitationModel = _scope.Resolve<InvitationModel>(new TypedParameter(typeof(string), email),
                    new TypedParameter(typeof(Guid), new Guid(projectId)));
                await invitationModel.SaveToInvitationAsync();
            }
        }

        public async Task<JsonResult> GetTeamMembersData(Guid Id)
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<ProjectListModel>();
            model.SearchItem = _scope.Resolve<ProjectSearch>();
            model.SearchItem.ProjectId = Id;
            return Json(await model.GetTeamMembersPagedData(dataTableModel));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Archive(Guid id)
        {
            try
            {
                var model = _scope.Resolve<ProjectViewModel>();
                await model.ArchiveProject(id);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfully Archived Project.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to archive project");

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in archiving Project.",
                    Type = ResponseTypes.Danger
                });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Invitation(Guid id)
        {
            var model = _scope.Resolve<InvitationResponseModel>();
            await model.LoadInvitationData(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Invitation(InvitationResponseModel model)
        {
            model.ResolveDependency(_scope);
            if (model.Status == InvitationStatus.Accepted)
            {
                await model.AcceptInvitation();
                return RedirectToAction(nameof(Details), new { id = model.ProjectId });
            }
            await model.RejectInvitation();
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Restore(Guid id)
        {
            try
            {
                var model = _scope.Resolve<ProjectListModel>();
                await model.RestoreProject(id);

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "Successfully Restored Project.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to restore project");

                TempData.Put<ResponseModel>("ResponseMessage", new ResponseModel
                {
                    Message = "There was a problem in restoring Project.",
                    Type = ResponseTypes.Danger
                });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}