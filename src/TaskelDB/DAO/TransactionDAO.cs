using MySqlConnector;
using TaskelDB.Models;
using TaskelDB.Models.Conversation;
using TaskelDB.Models.Service;
using TaskelDB.Models.User;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class TransactionDAO : BaseDAO<TransactionModel>
	{
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT
				id,
				receiver_id,
				sender_id,				
				send_time,
				cost,
				service_id
			FROM transactions
			WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
			SELECT
				id,
				receiver_id,
				sender_id,				
				send_time,
				cost,
				service_id
			FROM transactions";

        private static readonly string sqlCreateCmd = @"
			INSERT INTO transactions
			VALUES (@id, @receiver_id, @sender_id, @send_date, @send_time, @cost, @service_id)";

        private static readonly string sqlDeleteCmd = @"
			DELETE FROM transactions
			WHERE id = @id";

        private static readonly string sqlUpdateCmd = @"
			UPDATE transaction
			SET id = @id, 
				receiver_id = @receiver_id, 
				sender_id = @sender_id,
				send_time = @send_time,
				cost = @cost,
				service_id = @service_id,
			WHERE id = @id";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new Transaction entry in the database.
        /// </summary>
        public long Create(TransactionModel transaction)
		{
            return CreateElement(transaction, sqlCreateCmd);
        }

		/// <summary>
		/// Returns transaction from the database with target id.
		/// </summary>
		public TransactionModel? Get(long id)
		{
           return GetElement(id, sqlGetCmd);
		}

		/// <summary>
		/// Returns all transactionss from the database.
		/// </summary>
		public List<TransactionModel> GetAll()
		{
            return GetElements(sqlGetAllCmd);
		}

		/// <summary>
		/// Update row data of specified Transaction.
		/// </summary>
		public void Update(TransactionModel transaction)
		{
            UpdateElement(transaction, sqlUpdateCmd);
        }

		/// <summary>
		/// Deletes transaction with given id from the database
		/// </summary>
		public void Delete(long id)
		{
            DeleteElement(id, sqlDeleteCmd);
        }

        #endregion

        #region MAPPING 
        protected override TransactionModel MapSingle(MySqlDataReader reader)
        {
			return new TransactionModel()
			{
				ID = reader.GetInt32("id"),
				Receiver_ID = reader.TryGetInt32("receiver_id"),
				Sender_ID = reader.TryGetInt32("sender_id"),
				Send_Date = reader.TryGetDateOnly("send_date"),
				Send_Time = reader.TryGetTimeOnly("send_time"),
				Cost = reader.TryGetInt32("cost"),
				Service_ID = reader.TryGetInt32("service_id"),
            };
        }
        protected override DBParameters MapToParameters(TransactionModel model)
        {
			return new DBParameters()
				.AddParameter("id", model.ID)
				.AddParameter("receiver_id", model.Receiver_ID)
				.AddParameter("sender_id", model.Sender_ID)
				.AddParameter("send_date", model.Send_Date)
				.AddParameter("send_time", model.Send_Time)
                .AddParameter("cost", model.Cost)
				.AddParameter("service_id", model.Service_ID);
        }
        #endregion

		public void AddCreditsToUser(int userID, int serviceID, int amount)
		{
            if (amount <= 0)
            {
                throw new Exception("The transfer amount can't be equal to zero or be negative!");
            }

            var conn = DBConnection.Instance.GetConnection();
			var transaction = conn.BeginTransaction();

			try
			{
                UserDAO userDAO = new();
                userDAO.AddCreditsTransaction(conn, transaction, userID, amount);

				TransactionModel model = new()
				{
					Receiver_ID = userID,
					Sender_ID = 1,
					Send_Date = DateOnly.FromDateTime(DateTime.UtcNow),
					Send_Time = TimeOnly.FromDateTime(DateTime.UtcNow),
					Cost = amount,
					Service_ID = serviceID,
				};

				CreateElementTransaction(model, sqlCreateCmd, conn, transaction);
				transaction.Commit();
            }
			catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				transaction.Rollback();
			}	
        }
	}
}
