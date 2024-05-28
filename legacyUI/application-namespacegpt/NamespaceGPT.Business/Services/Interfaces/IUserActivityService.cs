using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IUserActivityService
    {
        int AddUserActivity(UserActivity userActivity);
        bool DeleteUserActivity(int id);
        bool UpdateUserActivity(int id, UserActivity userActivity);
        IEnumerable<UserActivity> GetAllUserActivities();
        IEnumerable<UserActivity> GetUserActivitiesOfUser(int userId);
    }
}
