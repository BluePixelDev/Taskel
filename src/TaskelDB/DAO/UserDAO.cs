using TaskelDB.Interfaces;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class UserDAO : IDAO<UserModel>
    {
        private static readonly string sqlGetCmd =
            "SELECT id, name, hashedPassword, current_credits, isAdmin " +
            "FROM users " +
            "WHERE id = @id";

        private static readonly string sqlGetAllCmd =
            "SELECT id, name, hashedPassword, current_credits, isAdmin " +
            "FROM users";

        private static readonly string sqlCreateCmd =
            "INSERT INTO users " +
            "VALUES (@id, @name, @hashedPassword, @current_credits, @isAdmin)";

        private static readonly string sqlDeleteCmd =
            "DELETE FROM users " +
            "WHERE id = @id";

        private static readonly string sqlUpdateCmd =
            "UPDATE users " +
            "SET id = @id, name = @name, hashedPassword = @hashedPassword " +
            "current_credits = @current_credits, isAdmin = @isAdmin " +
            "WHERE id = @id";

        private static readonly string sqlGetByEmail =
            "SELECT users.id, name, hashedPassword, current_credits, isAdmin " +
            "FROM users " +
            "LEFT JOIN email_addresses ON " +
            "email_addresses.user_id = users.id " +
            "WHERE email_address = @email_address";

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database.
        /// </summary>
        public long Create(UserModel user)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var data = DBMapper.MapToParameters(user);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlCreateCmd, data);
                var res = cmd.ExecuteScalar();

                var identity = DBUtility.GetLastID(conn);
                return identity != null ? Convert.ToInt64(identity) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
            return -1;
        }

        /// <summary>
        /// Returns user from the database with target id.
        /// </summary>
        public UserModel? Get(long id)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return reader.MapSingle<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        public List<UserModel> GetAll()
        {
            using var conn = DBConnection.Instance.GetConnection();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetAllCmd);
                using var reader = cmd.ExecuteReader();
                return reader.MapAll<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all users: {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Update row data of specified User.
        /// </summary>
        public void Update(UserModel user)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var parameters = DBMapper.MapToParameters(user);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlUpdateCmd, parameters);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user with id {user.ID}: {ex.Message}");
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
                Console.WriteLine($"Error deleting user with id {id}: {ex.Message}");
            }

        }
        #endregion

        /// <summary>
        /// Returns user from the database with specified email.
        /// </summary>
        public static UserModel? GetByEmail(string emailAddress)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("email_address", emailAddress);

            Console.WriteLine(sqlGetByEmail);
            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetByEmail, parameters);
                using var reader = cmd.ExecuteReader();
                return reader.MapSingle<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
            }
            return null;
        }
    }
}
