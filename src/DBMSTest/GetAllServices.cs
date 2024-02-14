using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerManagement;

namespace Tests
{
    [TestClass]
    public class GetAllServicesTest
    {
        [TestMethod]
        public void TestGettingAllServices()
        {
            // Arrange
            ServerManager manager = new();
            // Act
            var services = manager.GetAllServices();
            // Assert
            Assert.IsNotNull(services);
        }
    }
}
