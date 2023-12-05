using Autofac;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Codes;
using DevTrack.Web.Models;
using System.Security.Claims;

namespace DevTrack.Web.Areas.App.Models
{
    public class ProjectListModel : BaseModel
    {
        public ProjectSearch SearchItem { get; set; }

        private IProjectService _projectService;
        private IHttpContextAccessor _httpContextAccessor;

        public ProjectListModel() : base() { }

        public ProjectListModel(IProjectService projectService, IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _projectService = _scope.Resolve<IProjectService>();
        }

        internal async Task<object?> GetProjectsPagedData(DataTablesAjaxRequestModel dataTablesModel)
        {
            SearchItem.ApplicationUserId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = await _projectService.GetProjects(
                dataTablesModel.PageIndex,
                dataTablesModel.PageSize,
                dataTablesModel.SearchText,
                SearchItem.ApplicationUserId,
                SearchItem.Title,
                SearchItem.IsArchived,
                SearchItem.Role,
                dataTablesModel.GetSortText(new string[] { "Title", "Role", "TotalMembers", "TotalLogHours" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Title,
                                record.Role.ToString(),
                                record.TotalMembers.ToString(),
                                record.TotalLogHours.ToHourMinute(),
                                record.ProjectId.ToString(),
                                record.Status.ToString()
                        }
                    ).ToArray()
            };
        }

        internal async Task<object?> GetTeamMembersPagedData(DataTablesAjaxRequestModel dataTablesModel)
        {
            SearchItem.ApplicationUserId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = await _projectService.GetTeamMembers(
                dataTablesModel.PageIndex,
                dataTablesModel.PageSize,
                dataTablesModel.SearchText,
                SearchItem.ProjectId,
                SearchItem.ApplicationUserId,
                dataTablesModel.GetSortText(new string[] { "Name", "LoggedPastWeek", "LoggedThisMonth", "Status" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Name,
                            record.LoggedPastWeek.ToHourMinute(),
                            record.LoggedThisMonth.ToHourMinute(),
                            record.TotalLogged.ToHourMinute(),
                            record.Status.ToString()
                        }
                    ).ToArray()
            };
        }

        internal async Task RestoreProject(Guid id)
        {
            await _projectService.RestoreProject(id);
        }
    }
}