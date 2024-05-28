using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class UserActivityController
    {
        private readonly IUserActivityService userActivityService;

        public UserActivityController(IUserActivityService userActivityService)
        {
            this.userActivityService = userActivityService ?? throw new ArgumentNullException(nameof(userActivityService));
        }

        public int AddUserActivity(UserActivity userActivity)
        {
            return userActivityService.AddUserActivity(userActivity);
        }

        public bool DeleteUserActivity(int id)
        {
            return userActivityService.DeleteUserActivity(id);
        }

        public IEnumerable<UserActivity> GetAllUserActivities()
        {
            return userActivityService.GetAllUserActivities();
        }

        public IEnumerable<UserActivity> GetUserActivitiesOfUser(int userId)
        {
            return userActivityService.GetUserActivitiesOfUser(userId);
        }

        public bool UpdateUserActivity(int id, UserActivity userActivity)
        {
            return userActivityService.UpdateUserActivity(id, userActivity);
        }
    }
}
