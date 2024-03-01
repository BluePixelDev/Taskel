using MySqlConnector;
using TaskelDB;
using TaskelDB.DAO;
using TaskelDB.Models;

namespace TaskelDBTests
{
    [TestClass]
	public class UserTests
	{
		long userID = -1;

		[TestInitialize]
		public void Setup()
		{
			MySqlConnectionStringBuilder stringBuilder = new()
			{
				UserID = "ppraxe",
				Password = "Ppraxe+01",
				Database = "praxedb",
				Server = "93.99.225.235",
				ConnectionTimeout = 10,
			};

			DBConnection conn = DBConnection.Instance;
			conn.ConnectionString = stringBuilder.ConnectionString;
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