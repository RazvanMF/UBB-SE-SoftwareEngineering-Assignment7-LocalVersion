using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(User user);
        int UserExists(User user);
        bool DeleteUser(int id);
        bool UpdateUser(int id, User user);
        IEnumerable<User> GetAllUsers();
        User? GetUser(int id);
    }
}
