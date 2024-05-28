using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NamespaceGPT.Common.Modules.CustomLogging.Module;

namespace NamespaceGPT.Tests.Modules
{
    [TestClass]
    public class LoggingClassUnitTests
    {
        private string testLogFilePath = "testlogfile.txt"; // Test log file path

        [TestInitialize]
        public void Initialize()
        {
            // Set the test log file path
            LoggingClass.SetFilePath(testLogFilePath);
        }

        [TestMethod]
        public void TestLog_InfoType_MessageBuffered()
        {
            // Arrange
            string message = "Test info message";

            // Act
            LoggingClass.Log(LoggingClass.LogType.INFO, message);

            // Assert
            Queue<string> bufferedMessages = LoggingClass.GetBufferedMessages();
            Assert.IsTrue(bufferedMessages.Count > 0);
            Assert.IsTrue(bufferedMessages.Peek().Contains(message));
        }

        [TestMethod]
        public void TestLog_WarningType_MessageWrittenImmediately()
        {
            // Arrange
            string message = "Test warning message";

            // Act
            LoggingClass.Log(LoggingClass.LogType.WARNINGTYPE, message);

            // Assert
            Assert.IsTrue(File.ReadAllText(testLogFilePath).Contains(message));
        }

        [TestMethod]
        public void TestLog_ErrorType_MessageWrittenImmediately()
        {
            // Arrange
            string message = "Test error message";

            // Act
            LoggingClass.Log(LoggingClass.LogType.ERRORTYPE, message);

            // Assert
            Assert.IsTrue(File.ReadAllText(testLogFilePath).Contains(message));
        }

        [TestMethod]
        public void TestSetFilePath_NewFilePath_PathChanged()
        {
            // Arrange
            string newFilePath = "newlogfile.txt";

            // Act
            LoggingClass.SetFilePath(newFilePath);

            // Assert
            Assert.AreEqual(newFilePath, LoggingClass.GetFilePath());
        }

        [TestMethod]
        public void TestFlushMessages_OnSuccess_WriteToFile()
        {
            LoggingClass.Initialize();
            LoggingClass.TestFlush();
            Assert.IsTrue(File.ReadAllText(testLogFilePath).Contains("lorem"));
        }
    }
}
