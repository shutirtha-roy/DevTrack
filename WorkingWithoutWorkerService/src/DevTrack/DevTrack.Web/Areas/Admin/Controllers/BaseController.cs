using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.Admin.Controllers
{
    public class BaseController<T> : Controller
    {
        protected readonly ILifetimeScope _scope;
        protected readonly ILogger<T> _logger;

        public BaseController(ILifetimeScope scope, ILogger<T> logger)
        {
            _scope = scope;
            _logger = logger;
        }
    }
}