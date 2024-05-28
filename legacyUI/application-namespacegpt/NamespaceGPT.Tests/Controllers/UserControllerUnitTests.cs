using System.Diagnostics;
using System.Data;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using Microsoft.Data.SqlClient;

// CONTROLLER -> SERVICE -> REPOSITORY ARE CASCADING, TESTING THE TOP WILL GO ALL THE WAY TO THE BOTTOM
namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class UserControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM AppUser; DBCC CHECKIDENT('AppUser', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            SqlCommand addUserCommand = connection.CreateCommand();
            addUserCommand.CommandType = CommandType.Text;
            addUserCommand.CommandText = "INSERT INTO AppUser (username, password) VALUES (@username, @password);";
            addUserCommand.Parameters.AddWithValue("@username", "RazvanMF");
            addUserCommand.Parameters.AddWithValue("@password", "password");
            addUserCommand.ExecuteNonQuery();
            addUserCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }

        [TestMethod]
        public void TestAddUser_SuccessfulAdd_ReturnsUserId()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            User userToAdd = new User { Username = "added", Password = "added" };
            int result = userController.AddUser(userToAdd);
            Debug.Assert(result == 2); // if everything is fine, the user should be added, and 2 should be returned
            Debug.Assert(userController.LoginUser(userToAdd) == 2); // for completion purposes, the "LoginUser" function also checks this.
        }

        [TestMethod]
        public void TestGetUserViaID_SuccessfulFind_ReturnsUser()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            int userIDToFind = 1;
            User? foundUser = userService.GetUser(userIDToFind);
            Debug.Assert(foundUser != null && foundUser.Id == 1 && foundUser.Username == "RazvanMF" && foundUser.Password == "password");
        }

        [TestMethod]
        public void TestGetUserViaID_FailureFind_ReturnsNull()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            int nonExistentUserID = 5;
            User? nonExistentUser = userService.GetUser(nonExistentUserID);
            Debug.Assert(nonExistentUser == null);
        }

        [TestMethod]
        public void TestGetUserViaUsernameAndPassword_SuccessfulFind_ReturnsUserID()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            User userToFind = new User { Username = "RazvanMF", Password = "password" };
            int foundUserID = userController.LoginUser(userToFind);
            Debug.Assert(foundUserID == 1);
        }

        [TestMethod]
        public void TestGetUserViaUsernameAndPassword_FailureFind_ReturnsMinusOne()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            User nonExistentUserToFind = new User { Username = "I DO NOT EXIST", Password = "-" };
            int nonExistentFoundUserID = userController.LoginUser(nonExistentUserToFind);
            Debug.Assert(nonExistentFoundUserID == -1);
        }

        [TestMethod]
        public void TestUpdateUser_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            User userDataToUpdate = new User { Username = "UpdatedRazvanMF", Password = "parola" };
            int userIDToReplace = 1;
            bool updateState = userController.UpdateUser(userIDToReplace, userDataToUpdate);
            Debug.Assert(updateState == true);
            Debug.Assert(userController.LoginUser(userDataToUpdate) == userIDToReplace);
        }

        [TestMethod]
        public void TestUpdateUser_FailureUpdate_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            User userDataToUpdate = new User { Username = "UpdatedRazvanMF", Password = "parola" };
            int userIDToReplace = 5;
            bool updateState = userController.UpdateUser(userIDToReplace, userDataToUpdate);
            Debug.Assert(updateState == false);
            Debug.Assert(userController.LoginUser(userDataToUpdate) == -1);
        }

        [TestMethod]
        public void TestGetAllUsers_SuccessfulGet_ReturnsAllUsers()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            List<User> users = userController.GetAllUsers().ToList();
            Debug.Assert(users.Count == 1);
            Debug.Assert(users[0].Id == 1 && users[0].Username == "RazvanMF" && users[0].Password == "password");
        }

        [TestMethod]
        public void TestDeleteUser_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            int userIDToDelete = 1;
            bool deleteState = userController.DeleteUser(userIDToDelete);
            Debug.Assert(deleteState == true);
            Debug.Assert(userController.GetUser(userIDToDelete) == null);
        }

        [TestMethod]
        public void TestDeleteUser_FailureDelete_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserRepository userRepository = new UserRepository(configurationManager);
            IUserService userService = new UserService(userRepository);
            UserController userController = new UserController(userService);

            int userIDToDelete = 5;
            bool deleteState = userController.DeleteUser(userIDToDelete);
            Debug.Assert(deleteState == false);
        }

        [ClassCleanup]
        public static void RestoreUserData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM AppUser; DBCC CHECKIDENT('AppUser', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            SqlCommand addUserCommand = connection.CreateCommand();
            addUserCommand.CommandType = CommandType.Text;
            addUserCommand.CommandText = "INSERT INTO AppUser (username, password) VALUES (@username, @password);";
            addUserCommand.Parameters.AddWithValue("@username", "RazvanMF");
            addUserCommand.Parameters.AddWithValue("@password", "password");
            addUserCommand.ExecuteNonQuery();
            addUserCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }
    }
}

