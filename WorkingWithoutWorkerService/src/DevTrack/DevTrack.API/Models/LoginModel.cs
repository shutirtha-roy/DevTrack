using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DevTrack.API.Models
{
    public class LoginModel : BaseModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        private IAccountService _accountService;
        private IMapper _mapper;

        public LoginModel() : base() { }

        public LoginModel(IMapper mapper, IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public override void ResolveDependency(ILifetimeScope scope)
        {
            base.ResolveDependency(scope);
            _mapper = _scope.Resolve<IMapper>();
            _accountService = _scope.Resolve<IAccountService>();
        }

        internal async Task<LoginResponseModel> GetUserToken()
        {
            var model = new LoginResponseModel();
            var result = await _accountService.GetUserToken(Email, Password);

            if (result.isValidUser)
            {
                model.IsSuccess = true;
                model.StatusCode = (int)HttpStatusCode.OK;
                model.Data = _mapper.Map<UserInfo>(result.userInfo);
                model.Errors = new string[] { };
            }
            else
            {
                model.IsSuccess = false;
                model.StatusCode = (int)HttpStatusCode.BadRequest;
                model.Data = null;
                model.Errors = new string[] { "Invalid Credential" };
            }

            return model;
        }
    }
}