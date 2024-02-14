using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DAOS;
using DataAccessLibrary.DBChildManagers;
using DataTemplateLibrary.Models;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Security;

namespace DataAccessLibrary
{
    /// <summary>
    /// Cascade class for working with database and Session Service
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public class DBManager
    {
        private static DBManager? instance = null;
        private DBManager() { }

        private readonly DBUserManager userManager = new();
        private readonly DBServiceManager serviceManager = new();
        private readonly DBTransactionManager transManager = new();

        public static DBManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DBManager();
            }
            return instance;
        }

        public DBUser GetUserByName(string name)
        {
            return userManager.GetUserByName(name);
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
        /// Saves user to database and creates and retrievs his id
        /// </summary>
        /// <param name="user">User to save</param>
        /// <returns>Original user with updated id</returns>
        public DBUser? SingUpUser(DBUser user)
        {
            var data = userManager.SingUpUser(user);
            if (data != null) return data.Result;
            else throw new Exception(data.Message);
        }

        /// <summary>
        /// Reads database and returns a user with specific name
        /// </summary>
        /// <param name="name">Name of the user you want to read</param>
        /// <returns>User from database with the specified name</returns>
        public DBUser? ReadUserByName(string name)
        {
            return userManager.GetUserByName(name);
        }

        /// <summary>
        /// Check if user exists in the database and credentials
        /// </summary>
        /// <param name="user">Hypothetical user to check he exists in the database and if password is same</param>
        /// <returns>True in result if user exists and credentials are right, User from database.</returns>
        public DBUser? LogUserIn(DBUser user)
        {
            var data = userManager.LogUserIn(user);
            var user_from_db = data;
            if (user_from_db == null) throw new NullReferenceException("User doesnt exist in the database");
            return user_from_db;
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

        public DBService? GetServiceFromDB(int userId, int serviceId)
        {
            return serviceManager.GetOneServiceByUserIdAndServiceId(userId, serviceId);
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

        public List<DBService?> GetAllUserServices(int userId)
        {
            return serviceManager.GetAllServiceByUser(userId);
        }

        public bool CreateTransaction(DBTransaction transaction, int senderId, int recieverId, int amount)
        {
            return transManager.CreateTransaction(transaction, senderId, recieverId, amount);
        }

        public List<DBTransaction> ReadTransactionsByUserId(int userId)
        {
            return transManager.ReadTransactionsByUserId(userId);
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

        public void UpdateUser(int userId, DBUser newUserData)
        {
            userManager.UpdateUser(userId, newUserData);
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
