using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IUserActivityRepository
    {
        int AddUserActivity(UserActivity userActivity);
        bool DeleteUserActivity(int id);
        bool UpdateUserActivity(int id, UserActivity userActivity);
        IEnumerable<UserActivity> GetAllUserActivities();
        IEnumerable<UserActivity> GetUserActivitiesOfUser(int userId);
    }
}
