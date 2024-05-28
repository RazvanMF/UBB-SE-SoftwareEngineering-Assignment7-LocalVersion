using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class UserController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return userService.GetAllUsers();
        }

        public int AddUser(User user)
        {
            return userService.AddUser(user);
        }

        public int LoginUser(User user)
        {
            return userService.UserExists(user);
        }

        public bool DeleteUser(int id)
        {
            return userService.DeleteUser(id);
        }

        public User? GetUser(int id)
        {
            return userService.GetUser(id);
        }

        public bool UpdateUser(int id, User user)
        {
            return userService.UpdateUser(id, user);
        }
    }
}
