using Autofac;
using DevTrack.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevTrack.API.Controllers
{
    [Route("api/v1/[controller]")]
    [EnableCors("AllowSites")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly ILifetimeScope _scope;

        public AccountsController(ILogger<AccountsController> logger, ILifetimeScope scope) 
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpPost("Login")]
        public async Task<LoginResponseModel> Login(LoginModel model)
        {
            LoginResponseModel response = new LoginResponseModel();
            
            if (ModelState.IsValid)
            {
                try
                {
                    model.ResolveDependency(_scope);
                    response = await model.GetUserToken();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);

                    response.IsSuccess = false;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Data = null;
                    response.Errors = new string[] { "Login failed!" };
                }
            }
            else
            {
                response.IsSuccess = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Data = null;
                response.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
            }

            return response;
        }
    }
}