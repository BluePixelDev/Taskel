using MySqlConnector;
using TaskelDB.Models;
using TaskelDB.Models.Conversation;
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
            var conn = DBConnection.Instance.GetConnection();
			MySqlTransaction mySqlTransaction = conn.BeginTransaction();
            try
			{
				long elementID = CreateElement(transaction, sqlCreateCmd);
				mySqlTransaction.Commit();
				return elementID;
			}
			catch (Exception ex)
			{
				mySqlTransaction.Rollback();
				Console.WriteLine($"Error creating transaction: {ex.Message}");
			}
			finally
			{
				conn.Dispose();
			}
			return -1;
		}

		/// <summary>
		/// Returns transaction from the database with target id.
		/// </summary>
		public TransactionModel? Get(long id)
		{
            try
			{
				return GetElement(id, sqlGetCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error getting transaction: {ex.Message}");
			}
			return null;
		}

		/// <summary>
		/// Returns all transactionss from the database.
		/// </summary>
		public List<TransactionModel> GetAll()
		{
			try
			{
				return GetElements(sqlGetAllCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error getting all transactions: {ex.Message}");
			}

			return [];
		}

		/// <summary>
		/// Update row data of specified Transaction.
		/// </summary>
		public void Update(TransactionModel transaction)
		{
            using var conn = DBConnection.Instance.GetConnection();
            MySqlTransaction mySqlTransaction = conn.BeginTransaction();
            try
			{
				UpdateElement(transaction, sqlUpdateCmd);
                mySqlTransaction.Commit();
            }
			catch (Exception ex)
			{
				mySqlTransaction.Rollback();
                Console.WriteLine($"Error updating transaction with id {transaction.ID}: {ex.Message}");
			}
        }

		/// <summary>
		/// Deletes transaction with given id from the database
		/// </summary>
		public void Delete(long id)
		{
			try
			{
				DeleteElement(id, sqlDeleteCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting transaction with id {id}: {ex.Message}");
			}

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
				.AddParameter("send_time", model.Send_Time)
				.AddParameter("cost", model.Cost)
				.AddParameter("service_id", model.Service_ID);
        }
        #endregion
	}
}
