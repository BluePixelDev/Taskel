using TaskelDB;
using TaskelDB.DAO;
using TaskelDB.Models.User;

namespace TaskelDBTests.Tests
{
    [TestClass]
    public class EmailTests
    {
        long emailID = -1;

        [TestInitialize]
        public void Setup()
        {
            DBConnection conn = DBConnection.Instance;
            conn.ConnectionString = TestConfiguration.GetConnectionString();
		}

        [TestMethod("Email Creation")]
        public void CreateEmail()
        {
            EmailDAO emailDAO = new();
            EmailModel emailModel = new()
            {
                Email_Address = "testing@test.com",
                User_ID = 1
            };

            emailID = emailDAO.Create(emailModel);
            Assert.AreNotEqual(emailID, -1);
        }

        [TestMethod("Email Deletion")]
        public void DeleteUser()
        {
            EmailDAO emailDao = new();
            emailDao.Delete(emailID);
        }
    }
}
