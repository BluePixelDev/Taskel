using TaskelDB;
using TaskelDB.DAO;
using TaskelDB.Models.User;

namespace TaskelDBTests.Tests
{
    [TestClass]
    public class UserTests
    {
        long userID = -1;

        [TestInitialize]
        public void Setup()
        {
            DBConnection conn = DBConnection.Instance;
            conn.ConnectionString = TestConfiguration.GetConnectionString();
        }

        [TestMethod("User Creation")]
        public void CreateUser()
        {
            UserDAO userDAO = new();
            UserModel userModel = new()
            {
                Name = "DBTest",
                HashedPassword = "DBTest",
                Current_Credits = 0,
                IsAdmin = false,
            };

            userID = userDAO.Create(userModel);
            Console.WriteLine(userID);
            Assert.AreNotEqual(userID, -1);
        }

        [TestMethod("User Deletion")]
        public void DeleteUser()
        {
            UserDAO userDAO = new();
            userDAO.Delete(userID);
        }
    }
}