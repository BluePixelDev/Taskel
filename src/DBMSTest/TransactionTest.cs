using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using ServerManagement;

namespace Tests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TestingWithCorrectData()
        {
            // Arrange
            ServerManager serverManager = new ServerManager();
            Random randGenerator = new Random();
            DBUser sender = new DBUser($"{randGenerator.Next().ToString()} + testingSender", "testingSenderPass", 10);
            DBUser reciever = new DBUser($"{randGenerator.Next().ToString()} testingReciever", "testingRecieverPass", 0);
            sender = serverManager.SingUpUser(sender);
            reciever = serverManager.SingUpUser(reciever);
            var sessionIdSender = serverManager.LogUserInCreateSession(sender);
            DBService service = new DBService(reciever.ID,"testingTransactions",10,DateOnly.FromDateTime(DateTime.Now),null,false,"testingTransactions",null,null,false);
            service = serverManager.CreateService(sessionIdSender,service);
            DBTransaction transaction = new DBTransaction(reciever.ID,sender.ID,DateTime.Now,10,service.ID);
            // Act
            bool result = serverManager.CreateTransaction(sessionIdSender,transaction,reciever.ID);
            // Assert
            Assert.IsTrue(result);
        }
    }
}
