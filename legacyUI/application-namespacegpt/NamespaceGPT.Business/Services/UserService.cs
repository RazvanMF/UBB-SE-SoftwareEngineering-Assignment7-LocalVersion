using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public int AddUser(User user)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/users", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                User? result = JsonConvert.DeserializeObject<User>(responseBody);
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

            // return userRepository.AddUser(user);
        }

        public int UserExists(User user)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/users")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<User>? result = JsonConvert.DeserializeObject<List<User>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                List<User> parsedResult = result.Where(member => member.Username == user.Username && member.Password == user.Password).ToList();
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

            // return userRepository.UserExists(user);
        }

        public bool DeleteUser(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/users/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            // return userRepository.DeleteUser(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/users")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<User>? result = JsonConvert.DeserializeObject<List<User>>(responseBody);
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

            // return userRepository.GetAllUsers();
        }

        public User? GetUser(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/users" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<User>? result = JsonConvert.DeserializeObject<List<User>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                return result[0];
            }
            catch
            {
                return null;
            }

            // return userRepository.GetUser(id);
        }

        public bool UpdateUser(int id, User user)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/users" + id, content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                return true;
            }
            catch
            {
                return false;
            }

            // return userRepository.UpdateUser(id, user);
        }
    }
}
