using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DAOS;
using DataTemplateLibrary.Models;

namespace DataAccessLibrary.DBChildManagers
{
    /// <summary>
    /// This is a cascade that contains methods which are needed for DBManager.
    /// Works with DBService datatype
    /// </summary>
    /// <Creator>Anton Kalashnikov</Creator>
    public class DBServiceManager
    {
        ServiceDAO serviceDAO = new ServiceDAO();

        public ReturnData<DBService?, string> CreateService(DBService service)
        {
            int returnedID = serviceDAO.Create(service);
            service.ID = returnedID;
            return new ReturnData<DBService?, string>(service, "Created");
        }
        public List<DBService?> GetAllServiceByUser(int userId)
        {
            // TODO : Check for null and throw exception if null
            return serviceDAO.GetAllByUserID(userId);
        }

        // TODO: Watchout for overflow, reads all services on db, make a category or a read only 100 last!!!
        /// <summary>
        /// Gets all services from database 
        /// </summary>
        /// <returns>List of services</returns>
        public List<DBService?> GetAllServices()
        {
            return serviceDAO.GetAll();
        }

        public DBService? GetOneServiceByUserIdAndServiceId(int userId, int id)
        {
            foreach (var service in GetAllServiceByUser(userId))
            {
                if (service.ID == id) return service;
            }
            return null;
        }

        public DBService? GetService(int serviceId)
        {
            return serviceDAO.GetByID(serviceId);
        }

        public void UpdateService(int serviceId, DBService newService)
        {
            newService.ID = serviceId;
            newService.Updated = DateOnly.FromDateTime(DateTime.Now);
            serviceDAO.Save(newService);
        }

        internal bool ServiceExists(int serviceId)
        {
           return (serviceDAO.GetByID(serviceId) != null);
        }
    }
}
