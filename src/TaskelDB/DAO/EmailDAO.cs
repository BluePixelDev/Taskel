using MySqlConnector;
using TaskelDB.Interfaces;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class EmailDAO : IDAO<EmailModel>
    {
        private static readonly string sqlGetCmd =
            "SELECT id, email_address, user_id " +
            "FROM email_addresses " +
            "WHERE id = @id";

        private static readonly string sqlGetAllCmd =
            "SELECT id, email_address, user_id " +
            "FROM email_addresses";

        private static readonly string sqlCreateCmd =
            "INSERT INTO email_addresses " +
            "VALUES (@id, @email_address, @user_id)";

        private static readonly string sqlDeleteCmd =
            "DELETE FROM email_addresses " +
            "WHERE id = @id";

        private static readonly string sqlUpdateCmd =
            "UPDATE email_addresses " +
            "SET id = @id, email_address = @email_address, user_id = @user_id " +
            "WHERE id = @id";

        private static readonly string sqlGetByEmail =
            "SELECT id, email_address, user_id " +
            "FROM email_addresses " +
            "WHERE email_address = @email_address";

        private static readonly string sqlGetByEmailWithUser =
            "SELECT email_adresses.id, email_address, users.id, users.name" +
            "FROM email_addresses " +
            "WHERE email_address = @email_address";

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database. Email is automatically converted to lowercase.
        /// </summary>
        public long Create(EmailModel emails)
        {
            emails.Email_Address = emails.Email_Address.ToLower();
            using var conn = DBConnection.Instance.GetConnection();
            var data = MapToParameters(emails);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlCreateCmd, data);
                var res = cmd.ExecuteScalar();
                var identity = DBUtility.GetLastID(conn);
                return identity != null ? Convert.ToInt64(identity) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating email: {ex.Message}");
            }
            return -1;
        }

        /// <summary>
        /// Returns user from the database with target id.
        /// </summary>
        public EmailModel? Get(long id)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return MapSingle(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting email: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        public List<EmailModel> GetAll()
        {
            using var conn = DBConnection.Instance.GetConnection();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetAllCmd);
                using var reader = cmd.ExecuteReader();
                return MapAll(reader);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all email: {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Update row data of specified User.
        /// </summary>
        public void Update(EmailModel email)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var parameters = MapToParameters(email);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlUpdateCmd, parameters);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating email with id {email.ID}: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes user with given id from the database
        /// </summary>
        public void Delete(long id)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlDeleteCmd, parameters);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting email with id {id}: {ex.Message}");
            }

        }
        #endregion



        /// <summary>
        /// Returns EmailModel by the emailAddress.
        /// </summary>
        public EmailModel? GetByEmail(string emailAddress)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("email_address", emailAddress);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetByEmail, parameters);
                using var reader = cmd.ExecuteReader();
                return MapSingle(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting email: {ex.Message}");
            }
            return null;
        }


        public static List<EmailModel> MapAll(MySqlDataReader reader)
        {
            List<EmailModel> result = [];
            while (reader.Read())
            {
                result.Add(MapSingle(reader));
            }
            return result;
        }
        public static EmailModel MapSingle(MySqlDataReader reader)
        {
            return new EmailModel()
            {
                ID = reader.GetInt32("id"),
                User_ID = reader.GetInt32("user_id"),
                Email_Address = reader.GetString("email_address")
            };
        }
        public static DBParameters MapToParameters(EmailModel model)
        {
            DBParameters parameters = new();
            parameters.AddParameter("id", model.ID);
            parameters.AddParameter("user_id", model.User_ID);
            parameters.AddParameter("email_address", model.Email_Address);
            return parameters;
        }
    }
}
