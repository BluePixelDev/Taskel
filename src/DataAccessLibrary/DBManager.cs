using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;

namespace DataAccessLibrary
{
    /// <summary>
    /// Cascade class for working with database and Session Service
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    /// <colaborator>Ondrej Kacirek</colaborator>
    public class DBManager
    {
        private static DBManager? instance = null;
        private DBManager() { }

        private readonly DBUserManager userManager = new();
        private readonly DBServiceManager serviceManager = new();
        private readonly DBTransactionManager transManager = new();

        public static DBManager GetInstance()
        {
            instance ??= new DBManager();
            return instance;
        }
        
        public bool ServiceExists(int serviceId)
        {
            return serviceManager.ServiceExists(serviceId);
        }

        public bool UserOwnsService(int userId, int serviceId)
        {
            return (serviceManager.GetOneServiceByUserIdAndServiceId(userId,serviceId) != null);
        }

        /// <summary>
        /// Creates Service
        /// </summary>
        /// <param name="service">New service</param>
        /// <param name="userId">User id for service</param>
        /// <returns>Service with id from db</returns>
        public DBService? CreateService(DBService service, int userId)
        {
            service.UserId = userId;
            return serviceManager.CreateService(service).Result;
        }

        public DBService? GetServiceFromDB(int serviceId)
        {
            return serviceManager.GetService(serviceId);
        }

        public void UpdateService(int serviceId, DBService updatedService)
        {
            updatedService.ID = serviceId;
            serviceManager.UpdateService(serviceId, updatedService);
        }

        public bool CreateTransaction(DBTransaction transaction, int senderId, int recieverId, int amount)
        {
            return transManager.CreateTransaction(transaction, senderId, recieverId, amount);
        }

        public List<DBTransaction> ReadTransactionsByServiceId(int serviceId)
        {
            return transManager.ReadTransactionsByServiceId(serviceId);
        }

        public bool UserExists(int userId)
        {
            return userManager.UserExists(userId);
        }

        public DBUser GetUser(int userId)
        {
            return userManager.GetUserById(userId);
        }

        public List<DBService?> GetAllServices()
        {
            return serviceManager.GetAllServices();
        }

        public DBUser? AddCreditsToUser(int userId, int amount)
        {
            DBUser? admin = userManager.GetUserByName("Admin");
            // FIXME: If enough time find a better way to send transactions without admin and etc. Might need to revamp the DB.
            if (admin == null) throw new Exception("Admin doesnt exist in the database, cant complete the transaction");
            if (!transManager.AddCreditToUser(userId, admin.ID, amount)) throw new Exception("Wasnt able to add credits to user, problem with database");
            return userManager.GetUserById(userId);
        }

        public int GetAmountOfBuys(int serviceId, int days)
        {
            return transManager.GetAmountOfBuys(serviceId, days);
        }

        public int GetMoneyObtained(int serviceId)
        {
            return transManager.GetTotalMoney(serviceId);
        }
    }
}
