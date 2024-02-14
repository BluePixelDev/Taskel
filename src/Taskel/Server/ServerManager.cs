using DataAccessLibrary;
using DataTemplateLibrary.Models;
using SessionService;

namespace ServerManagement
{
    public class ServerManager
    {
        private readonly DBManager dbManager = DBManager.GetInstance();
        private readonly SessionManager sessionManager = SessionManager.Instance;


        // Methods that work with session id as an atribute:

        public DBUser GetUserBySessionId(string sessionId)
        {
            CheckSessionExistance(sessionId);
            return dbManager.GetUser(sessionManager.GetUserIdFromSessionId(sessionId));
        }

        /// <summary>
        /// Adds service to bd with id of the user
        /// </summary>
        /// <param name="id">Session id for ServerSideSessionSaverService</param>
        /// <param name="service">Service you want to save without userId</param>
        /// <returns>Returns service with id from db</returns>
        /// <exception cref="Exception">If session doesnt exists</exception>
        public DBService? CreateService(string sessionId, DBService service)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            service.UserId = userId;
            return dbManager.CreateService(service, userId);
        }

        /// <summary>
        /// Gets all user services from database
        /// </summary>
        /// <param name="sessionId">Session id of the user</param>
        /// <returns>List of services with their ids from db </returns>
        /// <exception cref="Exception">If session wasnt found</exception>
        public List<DBService?> GetAllUserServices(string sessionId)
        {
            CheckSessionExistance(sessionId);
            return dbManager.GetAllUserServices(sessionManager.GetUserIdFromSessionId(sessionId));
        }

        public DBService? GetService(string sessionId, int serviceId)
        {
            CheckSessionExistance(sessionId);
            return dbManager.GetServiceFromDB(sessionManager.GetUserIdFromSessionId(sessionId), serviceId);
        }
        public DBService? GetService(int serviceId)
        {
            return dbManager.GetServiceFromDB(serviceId);
        }

        /// <summary>
        /// Update service on db using service id and session id
        /// </summary>
        /// <param name="sessionId">Current user session id</param>
        /// <param name="serviceId">Service id of the service to be updated</param>
        /// <param name="updatedService">New service with updated values</param>
        /// <exception cref="Exception"></exception>
        public void UpdateService(string sessionId, int serviceId, DBService updatedService)
        {
            CheckSessionExistance(sessionId);
            updatedService.ID = serviceId;
            dbManager.UpdateService(serviceId, updatedService);
        }

        /// <summary>
        /// Creates a transaction for database, and sets values to users cretids
        /// </summary>
        /// <param name="sessionId">Session id of the user who sends the money</param>
        /// <param name="transaction">Transaction data</param>
        /// <param name="recieverId">To be sure that the action has a reciever id</param>
        /// <returns></returns>
        public bool CreateTransaction(string sessionId, DBTransaction transaction, int recieverId)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            if (!dbManager.UserExists(userId)) throw new Exception($"Sending user doesnt exist in the database, cant create a transaction from non existing user");
            if (!dbManager.UserExists(recieverId)) throw new Exception("User doesnt exist in database, cant create a transaction to a none existing user");
            DBUser user = dbManager.GetUser(userId);
            DBUser recievingUser = dbManager.GetUser(recieverId);
            if (user.CurrentCredits < transaction.Amount) throw new Exception($"User doesnt have enough credits to send the transaction {user.CurrentCredits} / {transaction.Amount}");
            return dbManager.CreateTransaction(transaction, user.ID, recieverId, Math.Abs(transaction.Amount));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public int GetUserIdFromSessionId(string sessionId)
        {
            CheckSessionExistance(sessionId);
            return sessionManager.GetUserIdFromSessionId(sessionId);
        }

        /// <summary>
        /// Creates a transaction and updates money on buyer side.
        /// Transaction sender is admin.
        /// </summary>
        /// <param name="sessionId">User session id</param>
        /// <param name="amount">Amount of buying</param>
        /// <returns></returns>
        public DBUser? BuyCredits(string sessionId, int amount)
        {
            CheckSessionExistance(sessionId);
            if (amount < 1) throw new Exception("You cant buy zero or negative credits");
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            // Money API logic (not adding)
            DBUser? updatedUser = dbManager.AddCreditsToUser(userId, amount);
            return updatedUser;
        }

        // Methods that work with user :

        public DBUser GetUserByName(string name)
        {
           return dbManager.GetUserByName(name);
        }

        /// <summary>
        /// Checks if user exists and credentials are right, asks for a new session id and puts it into the session manager
        /// </summary>
        /// <param name="user">Hypothetical user to check if the credentials are right</param>
        /// <returns>Session id in Result and User from database in Message</returns>
        /// <exception cref="Exception"></exception>
        public string LogUserInCreateSession(DBUser user)
        {
            DBUser? returned_user_data = dbManager.LogUserIn(user);
            if(returned_user_data != null)
            {
                string sessionId = sessionManager.GenerateNewSession(returned_user_data.ID);
                return sessionId;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DBUser? SingUpUser(DBUser user)
        {
            return dbManager.SingUpUser(user);
        }

        public DBUser GetUserById(int userId)
        {
           return dbManager.GetUser(userId);
        }

        // Methods that work with services :

        /// <summary>
        /// This method is used for testing only
        /// </summary>
        /// <returns>ALL services that exist in the database</returns>
        [Obsolete("Method is used for testing purposes, will need a remake in future")]
        public List<DBService?> GetAllServices()
        {
            return dbManager.GetAllServices();
        }

        // Transaction statistics Methods : 

        public int GetAmountOfBuys(EnumAnaliticsTimeSpan time, string sessionId, int serviceId)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            CheckServiceExists(serviceId);
            CheckUserOwnsService(userId, serviceId);
            return dbManager.GetAmountOfBuys(serviceId,(int)time);
        }

        public int GetTotalMoneyRecieved(string sessionId, int serviceId)
        {
            CheckSessionExistance(sessionId);
            int userId = sessionManager.GetUserIdFromSessionId(sessionId);
            CheckServiceExists(serviceId);
            CheckUserOwnsService(userId, serviceId);
            return dbManager.GetMoneyObtained(serviceId);
        }

        // Private Methods

        private void CheckSessionExistance(string sessionId)
        {
            if (!sessionManager.SessionExists(sessionId)) throw new Exception($"Session with {sessionId} session id doensnt exist");
        }

        private void CheckServiceExists(int serviceId)
        {
            if (!dbManager.ServiceExists(serviceId)) throw new Exception("Service doesnt exist");
        }

        private void CheckUserOwnsService(int userId,int serviceId)
        {
            if (!dbManager.UserOwnsService(userId, serviceId)) throw new Exception("User doesnt own the service");
        }
    }
}