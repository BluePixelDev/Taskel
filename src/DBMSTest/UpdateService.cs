using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using ServerManagement;

namespace Tests
{
    /// <summary>
    /// Test for updating service on database
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    [TestClass]
    public class UpdateServiceTest
    {
        [TestMethod]
        public void UpdateServiceTesting()
        {
            // Arrange
            ServerManager manager = new ServerManager();
            Random rand = new Random();
            string name = rand.Next().ToString();
            string pass = rand.Next().ToString();
            DBUser user = new DBUser(name, pass);
            user = manager.SingUpUser(user);
            DBService service = new DBService(user.ID, "test", 0, DateOnly.Parse("2023-05-18"), null, true, "testingtesting", null, null,false);
            DBService updating_to = new DBService(user.ID, "updatedtest", 10, DateOnly.Parse("2000-01-01"), null, true, "updatedtest", null, null,false);
            // Act
            var id = manager.LogUserInCreateSession(user);
            DBService service_from_db = manager.CreateService(id,service);
            manager.UpdateService(id, service_from_db.ID, updating_to);
            updating_to.ID = service_from_db.ID;
            DBService updated_service_from_db = manager.GetService(id,updating_to.ID);
            // Assert
            Assert.AreEqual(updating_to.ServiceName,updated_service_from_db.ServiceName);
        }
    }
}
