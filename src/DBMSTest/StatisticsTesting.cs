using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataTemplateLibrary.Models;
using ServerManagement;

namespace Tests
{
    [TestClass]
    public class StatisticsTesting
    {
        [TestMethod]
        public void TestingAmountsOfReservingDayWholeYear()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random random = new Random();
            DBUser serviceHolder = new DBUser($"serviceHolder:{random.Next().ToString()}","test",0);
            DBUser serviceBuyer = new DBUser($"serviceBuyer:{random.Next().ToString()}","test",100);
            serviceHolder = manager.SingUpUser(serviceHolder);
            serviceBuyer = manager.SingUpUser(serviceBuyer);
            DBService service = new DBService(serviceHolder.ID,"testingStatistics",10,DateOnly.FromDateTime(DateTime.Now),null,true,"test",null,null,false);
            string holderSessionId = manager.LogUserInCreateSession(serviceHolder);
            string buyerSessionId = manager.LogUserInCreateSession(serviceBuyer);
            service = manager.CreateService(holderSessionId, service);
            // Act
            for (int i = 8; i< 9; i++)
            {
                manager.CreateTransaction(buyerSessionId, new DBTransaction(serviceHolder.ID, serviceBuyer.ID, DateTime.Parse($"5/23/20{(15+i).ToString()}"), 10, service.ID),serviceHolder.ID);
            }
            int count = manager.GetAmountOfBuys(EnumAnaliticsTimeSpan.Year, holderSessionId, service.ID);
            // Assert
            Assert.AreEqual(1,count);
        }

        [TestMethod]
        public void TestingAmountsOfReservingDayWholeDay()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random random = new Random();
            DBUser serviceHolder = new DBUser($"serviceHolder:{random.Next().ToString()}", "test", 0);
            DBUser serviceBuyer = new DBUser($"serviceBuyer:{random.Next().ToString()}", "test", 100);
            serviceHolder = manager.SingUpUser(serviceHolder);
            serviceBuyer = manager.SingUpUser(serviceBuyer);
            DBService service = new DBService(serviceHolder.ID, "testingStatistics", 10, DateOnly.FromDateTime(DateTime.Now), null, true, "test", null, null, false);
            string holderSessionId = manager.LogUserInCreateSession(serviceHolder);
            string buyerSessionId = manager.LogUserInCreateSession(serviceBuyer);
            service = manager.CreateService(holderSessionId, service);
            // Act
            for (int i = 7; i < 9; i++)
            {
                manager.CreateTransaction(buyerSessionId, new DBTransaction(serviceHolder.ID, serviceBuyer.ID, DateTime.Parse($"5/25/20{(15 + i).ToString()}"), 10, service.ID), serviceHolder.ID);
            }
            int count = manager.GetAmountOfBuys(EnumAnaliticsTimeSpan.Day, holderSessionId, service.ID);
            // Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestingAmountsOfReservingDayWholeMonth()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random random = new Random();
            DBUser serviceHolder = new DBUser($"serviceHolder:{random.Next().ToString()}", "test", 0);
            DBUser serviceBuyer = new DBUser($"serviceBuyer:{random.Next().ToString()}", "test", 100);
            serviceHolder = manager.SingUpUser(serviceHolder);
            serviceBuyer = manager.SingUpUser(serviceBuyer);
            DBService service = new DBService(serviceHolder.ID, "testingStatistics", 10, DateOnly.FromDateTime(DateTime.Now), null, true, "test", null, null, false);
            string holderSessionId = manager.LogUserInCreateSession(serviceHolder);
            string buyerSessionId = manager.LogUserInCreateSession(serviceBuyer);
            service = manager.CreateService(holderSessionId, service);
            // Act
            for (int i = 8; i < 9; i++)
            {
                manager.CreateTransaction(buyerSessionId, new DBTransaction(serviceHolder.ID, serviceBuyer.ID, DateTime.Parse($"5/23/20{(15 + i).ToString()}"), 10, service.ID), serviceHolder.ID);
            }
            int count = manager.GetAmountOfBuys(EnumAnaliticsTimeSpan.Month, holderSessionId, service.ID);
            // Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod] 
        public void TestingAmountOfMoneyObtainedResult10()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random random = new Random();
            DBUser serviceHolder = new DBUser($"serviceHolder:{random.Next().ToString()}", "test", 0);
            DBUser serviceBuyer = new DBUser($"serviceBuyer:{random.Next().ToString()}", "test", 100);
            serviceBuyer = manager.SingUpUser(serviceBuyer);
            serviceHolder = manager.SingUpUser(serviceHolder);
            DBService service = new DBService(serviceHolder.ID, "testingStatistics", 10, DateOnly.FromDateTime(DateTime.Now), null, true, "test", null, null, false);
            string holderSessionId = manager.LogUserInCreateSession(serviceHolder);
            string buyerSessionId = manager.LogUserInCreateSession(serviceBuyer);
            service = manager.CreateService(holderSessionId, service);
            // Act
            for (int i = 0; i < 5; i++)
            {
                manager.CreateTransaction(buyerSessionId, new DBTransaction(serviceHolder.ID, serviceBuyer.ID, DateTime.Parse($"5/23/20{(15 + i).ToString()}"), 2, service.ID), serviceHolder.ID);
            }
            int money = manager.GetTotalMoneyRecieved(holderSessionId, service.ID);
            // Assert
            Assert.AreEqual(10, money);
        }
    }
}
