using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class UserActivityControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM UserActivity; DBCC CHECKIDENT('UserActivity', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();
            SqlCommand addUserActivityCommand = connection.CreateCommand();
            addUserActivityCommand.CommandType = CommandType.Text;
            addUserActivityCommand.CommandText = "INSERT INTO UserActivity (userID, actionType) " +
                "VALUES (@userID, @actionType);";
            addUserActivityCommand.Parameters.AddWithValue("@userID", "1");
            addUserActivityCommand.Parameters.AddWithValue("@actionType", "type1");
            addUserActivityCommand.ExecuteNonQuery();
            addUserActivityCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }

        [TestMethod]
        public void TestAddUserActivity_SuccessfulAdd_ReturnsUserActivityID()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            UserActivity userActivity = new UserActivity
            {
                Id = 2,
                UserId = 1,
                ActionType = "action"
            };

            int addedUserActivityId = userActivityController.AddUserActivity(userActivity);
            Console.WriteLine(addedUserActivityId);
            Debug.Assert(addedUserActivityId == 2);
            List<UserActivity> retrievedUserActivities = userActivityController.GetUserActivitiesOfUser(1).ToList();
            UserActivity? retrievedUserActivity = retrievedUserActivities[1];
            Debug.Assert(
                retrievedUserActivity != null &&
                retrievedUserActivity.Id == 2 &&
                retrievedUserActivity.UserId == 1 &&
                retrievedUserActivity.ActionType == "action");
        }

        [TestMethod]
        public void TestGetUserActivity_SuccessfulGet_ReturnsUserActivity()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            int idToFind = 1;
            List<UserActivity> retrievedUserActivities = userActivityController.GetUserActivitiesOfUser(idToFind).ToList();
            UserActivity? userActivityFound = retrievedUserActivities[0];
            Debug.Assert(userActivityFound != null);
            Debug.Assert(userActivityFound.Id == 1);
            Debug.Assert(userActivityFound.UserId == 1);
            Debug.Assert(userActivityFound.ActionType == "type1");
        }

        [TestMethod]
        public void TestGetUserActivity_FailureGet_ReturnsNull()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            int idToFind = 1987;
            List<UserActivity> retrievedUserActivities = userActivityController.GetUserActivitiesOfUser(idToFind).ToList();
            Debug.Assert(retrievedUserActivities.Count() == 0);
        }

        [TestMethod]
        public void TestDeleteUserActivity_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            int idToDelete = 1;
            bool deleteState = userActivityController.DeleteUserActivity(idToDelete);
            Assert.IsTrue(deleteState);
            Debug.Assert(userActivityController.GetUserActivitiesOfUser(idToDelete).Count() == 0);
        }

        [TestMethod]
        public void TestDeleteUserActivity_SuccessfulDelete_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            int idToDelete = 1987;
            bool deleteState = userActivityController.DeleteUserActivity(idToDelete);
            Assert.IsFalse(deleteState);
        }

        [TestMethod]
        public void TestUpdateUserActivity_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            UserActivity userActivityEntryToBeModifiedInto = new UserActivity
            {
                UserId = 1,
                ActionType = "action"
            };

            int idOfUserActivityToBeUpdated = 1;
            bool updateState = userActivityController.UpdateUserActivity(idOfUserActivityToBeUpdated, userActivityEntryToBeModifiedInto);
            Assert.IsTrue(updateState);
            List<UserActivity> retrievedUserActivities = userActivityController.GetUserActivitiesOfUser(idOfUserActivityToBeUpdated).ToList();
            UserActivity? retrievedUserActivity = retrievedUserActivities[0];
            Debug.Assert(
                retrievedUserActivity != null &&
                retrievedUserActivity.Id == 1 &&
                retrievedUserActivity.UserId == 1 &&
                retrievedUserActivity.ActionType == "action");
        }

        [TestMethod]
        public void TestUpdateMarkeplace_FailureUpdate_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            UserActivity userActivityEntryToBeModifiedInto = new UserActivity
            {
                UserId = 1,
                ActionType = "action"
            };

            int idOfUserActivityToBeUpdated = 1987;
            bool updateState = userActivityController.UpdateUserActivity(idOfUserActivityToBeUpdated, userActivityEntryToBeModifiedInto);
            Assert.IsFalse(updateState);
        }

        [TestMethod]
        public void TestGetAllUserActivity_SuccessfulGet_ReturnsAllUserActivitys()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IUserActivityRepository userActivityRepository = new UserActivityRepository(configurationManager);
            IUserActivityService userActivityService = new UserActivityService(userActivityRepository);
            UserActivityController userActivityController = new UserActivityController(userActivityService);

            List<UserActivity> userActivities = userActivityController.GetAllUserActivities().ToList();
            Debug.Assert(userActivities.Count == 1);
            Debug.Assert(userActivities[0].Id == 1 &&
                userActivities[0].UserId == 1 &&
                userActivities[0].ActionType == "type1");
        }

        [ClassCleanup]
        public static void RestoreUserActivityData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM UserActivity; DBCC CHECKIDENT('UserActivity', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }
    }
}
