using AutoMapper;
using DevTrack.Infrastructure.UnitOfWorks;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationUserManager _applicationUserManager;

        public UserService(IMapper mapper, IApplicationUnitOfWork applicationUnitOfWork, ApplicationUserManager applicationUserManager)
        {
            _mapper = mapper;
            _applicationUserManager = applicationUserManager;
        }

        public async Task<IList<ApplicationUserBO>> GetApplicationUsers()
        {
            var adminUser = await _applicationUserManager.GetUsersInRoleAsync("Admin");
            var adminId = adminUser.FirstOrDefault()?.Id;

            var users = _applicationUserManager.Users.Where(x => x.Id != adminId).ToList();

            var usersList = new List<ApplicationUserBO>();

            foreach (var user in users)
            {
                var userBO = _mapper.Map<ApplicationUserBO>(user);
                usersList.Add(userBO);
            }

            return usersList;
        }

        public async Task<ApplicationUserBO> GetUserDetails(Guid userId)
        {
            var userEntity = await _applicationUserManager.FindByIdAsync(userId.ToString());

            if (userEntity == null)
                throw new Exception("Unable to fetch user data");

            var user = _mapper.Map<ApplicationUserBO>(userEntity);

            return user;
        }
    }
}