using MySqlConnector;
using TaskelDB.Interfaces;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class TransactionDAO : IDAO<TransactionModel>
    {
        private static readonly string sqlGetCmd =
            "SELECT id, reciever_id, sender_id, send_date, send_time, cost, service_id " +
            "FROM transactions " +
            "WHERE id = @id";

        private static readonly string sqlGetAllCmd =
            "SELECT id, reciever_id, sender_id, send_date, send_time, cost, service_id " +
            "FROM transactions";

        private static readonly string sqlCreateCmd =
            "INSERT INTO transactions " +
            "VALUES (@id, @reciever_id, @sender_id, @send_date, @send_time, @cost, @service_id)";

        private static readonly string sqlDeleteCmd =
            "DELETE FROM transactions " +
            "WHERE id = @id";

        private static readonly string sqlUpdateCmd =
            "UPDATE transactions " +
            "SET id = @id, reciever_id = @user_id, sender_id = @ser_name, send_date = @current_price, send_time = @creation, cost = @update, service_id = @isShown, " +
            "current_credits = @current_credits, isAdmin = @isAdmin, " +
            "short_description = @short_description, long_description = @long_description, link = @link, isDeleted = @isDeleted, category = @category " +
            "WHERE id = @id";

        #region CORE DAO
        /// <summary>
        /// Creates new service entry in the database.
        /// </summary>
        public long Create(TransactionModel service)
        {
            var parameters = DBMapper.MapToParameters(service);
            using var conn = DBConnection.Instance.GetConnection();
            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlCreateCmd, parameters);
                var res = cmd.ExecuteScalar();
                var identity = DBUtility.GetLastID(conn);
                transaction.Commit();
                return identity != null ? Convert.ToInt64(identity) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating transaction: {ex.Message}");
                transaction.Rollback();
            }
            return -1;
        }

        /// <summary>
        /// Returns service from the database with target id.
        /// </summary>
        public TransactionModel? Get(long id)
        {
            DBParemeters parameters = new();
            parameters.AddParameter("id", id);
            using var conn = DBConnection.Instance.GetConnection();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return reader.MapSingle<TransactionModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transaction: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Returns all services from the database.
        /// </summary>
        public List<TransactionModel> GetAll()
        {
            using var conn = DBConnection.Instance.GetConnection();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetAllCmd);
                using var reader = cmd.ExecuteReader();
                return reader.MapAll<TransactionModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all transaction: {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Updates row data of specified service.
        /// </summary>
        public void Update(TransactionModel service)
        {
            var data = DBMapper.MapToParameters(service);
            using var conn = DBConnection.Instance.GetConnection();
            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlUpdateCmd, data);
                cmd.ExecuteScalar();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating transaction with id {service.ID}: {ex.Message}");
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Deletes service with given id from the database
        /// </summary>
        public void Delete(long id)
        {
            DBParemeters parameters = new();
            parameters.AddParameter("id", id);
            using var conn = DBConnection.Instance.GetConnection();
            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlDeleteCmd, parameters);
                cmd.ExecuteScalar();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction with id {id}: {ex.Message}");
                transaction.Rollback();
            }

        }
        #endregion
    }
}
