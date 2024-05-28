using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository userRepository;

        public UserActivityService(IUserActivityRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public int AddUserActivity(UserActivity userActivity)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(userActivity), Encoding.UTF8, "application/json");
                content.GetType();

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/useractivities", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                UserActivity? result = JsonConvert.DeserializeObject<UserActivity>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                return result.Id;
            }
            catch
            {
                return -1;
            }
        }

        public int UserActivityExists(UserActivity userActivity)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/useractivities")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<UserActivity>? result = JsonConvert.DeserializeObject<List<UserActivity>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                result = result.ToList();

                List<UserActivity> parsedResult = new List<UserActivity>();

                foreach (UserActivity activity in result)
                {
                    if (activity != null && activity.UserId == userActivity.UserId && activity.ActionType == userActivity.ActionType)
                    {
                        parsedResult.Add(activity);
                    }
                }

                if (parsedResult.Count > 0)
                {
                    return parsedResult[0].Id;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteUserActivity(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/useractivities/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<UserActivity> GetAllUserActivities()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/useractivities")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<UserActivity>? result = JsonConvert.DeserializeObject<List<UserActivity>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public UserActivity? GetUserActivity(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/useractivities" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                UserActivity? result = JsonConvert.DeserializeObject<UserActivity>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateUserActivity(int id, UserActivity userActivity)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(userActivity), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/useractivities" + id, content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                return true;
            }
            catch
            {
                return false;
            }
        }

        IEnumerable<UserActivity> IUserActivityService.GetUserActivitiesOfUser(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/useractivities")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<UserActivity>? allActivities = JsonConvert.DeserializeObject<List<UserActivity>>(responseBody);
                if (allActivities == null)
                {
                    throw new Exception("???");
                }

                List<UserActivity>? userActivity = new List<UserActivity>();

                foreach (UserActivity activity in allActivities)
                {
                    if (activity != null && activity.UserId == id)
                    {
                        userActivity.Add(activity);
                    }
                }

                return userActivity;
            }
            catch
            {
                return null;
            }
        }
    }
}
