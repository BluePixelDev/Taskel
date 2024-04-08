using TaskelDB;
using TaskelDB.DAO;
using TaskelDB.Models;

namespace TaskelDBTests.Tests
{
    [TestClass]
    public class TransactionTests
    {
        [TestInitialize]
        public void Setup()
        {
            DBConnection conn = DBConnection.Instance;
            conn.ConnectionString = TestConfiguration.GetConnectionString();
        }

        [TestMethod("Transaction Creation")]
        public void CreateTransaction()
        {
            TransactionDAO transactionDAO = new();
            TransactionModel transactionModel = new()
            {
                Receiver_ID = 1,
                Sender_ID = 2,
                Cost = 100,
                Service_ID = 0
            };
            long transactionID = transactionDAO.Create(transactionModel);

            Assert.AreNotEqual(-1, transactionID);
        }
    }
}
