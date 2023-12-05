using Autofac;
using DevTrack.Infrastructure.Services;
using System.Security.Claims;

namespace DevTrack.Web.Models
{
    public class BaseModel
    {
        protected ILifetimeScope _scope;
        public ITimeService _timeService;
        public IHttpContextAccessor _httpContextAccessor;

        public BaseModel()
        {

        }

        public virtual void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _timeService = _scope.Resolve<ITimeService>();
        }
    }
}