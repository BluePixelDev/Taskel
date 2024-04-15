using MySqlConnector;
using TaskelDB.Models.User;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class UserDAO : BaseDAO<UserModel>
    {
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT
				id,
				name,
				hashedPassword,
				current_credits,
				isAdmin
			FROM users
			WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
			SELECT
				id,
				name,
				hashedPassword,
				current_credits,
				isAdmin
			FROM users";

        private static readonly string sqlCreateCmd = @"
			INSERT INTO users
			VALUES (@id, @name, @hashedPassword, @current_credits, @isAdmin)";

        private static readonly string sqlDeleteCmd = @"
			DELETE FROM users
			WHERE id = @id";

        private static readonly string sqlUpdateCmd = @"
			UPDATE users
			SET id = @id, 
				name = @name, 
				hashedPassword = @hashedPassword,
				current_credits = @current_credits,
				isAdmin = @isAdmin
			WHERE id = @id";

        private static readonly string sqlGetByEmailCmd = @"
			SELECT
				users.id,
				name,
				hashedPassword,
				current_credits,
				isAdmin
			FROM users
				LEFT JOIN email_addresses ON email_addresses.user_id = users.id
			WHERE email_address = @email_address";

        private static readonly string sqlAddCreditsCmd = @"
			UPDATE 
				users
			SET 
				current_credits = current_credits + @transfer_amount
			WHERE 
				id = @user_id;";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database.
        /// </summary>
        public long Create(UserModel user)
        {
            return CreateElement(user, sqlCreateCmd);
        }

        /// <summary>
        /// Returns user from the database with target id.
        /// </summary>
        public UserModel? Get(long id)
        {
            return GetElement(id, sqlGetCmd);
        }

        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        public List<UserModel> GetAll()
        {
            try
            {
                return GetElements(sqlGetAllCmd);
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
            UpdateElement(user, sqlUpdateCmd);
        }

        /// <summary>
        /// Deletes user with given id from the database
        /// </summary>
        public void Delete(long id)
        {
            DeleteElement(id, sqlDeleteCmd);
        }

        #endregion

        #region MAPPING 
        protected override UserModel MapSingle(MySqlDataReader reader)
        {
            return new UserModel()
            {
                ID = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Current_Credits = reader.GetInt32("current_credits"),
                HashedPassword = reader.GetString("hashedPassword"),
                IsAdmin = reader.GetBoolean("isAdmin")
            };
        }
        protected override DBParameters MapToParameters(UserModel model)
        {
            return new DBParameters()
                .AddParameter("id", model.ID)
                .AddParameter("name", model.Name)
                .AddParameter("current_credits", model.Current_Credits)
                .AddParameter("hashedPassword", model.HashedPassword)
                .AddParameter("isAdmin", model.IsAdmin);
        }
        #endregion

        /// <summary>
        /// Returns user from the database with specified email.
        /// </summary>
        public UserModel? GetUserByEmail(string emailAddress)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("email_address", emailAddress);

            using var cmd = DBUtility.CreateCommand(conn, sqlGetByEmailCmd, parameters);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapSingle(reader);
            }

            return null;
        }

        public static void AddCreditsTransaction(MySqlConnection conn, MySqlTransaction transaction, int userID, int amount)
        {
            DBParameters parameters = new();
            parameters.AddParameter("user_id", userID);
            parameters.AddParameter("transfer_amount", amount);
            using var cmd = DBUtility.CreateCommand(conn, sqlAddCreditsCmd, parameters);
            cmd.Transaction = transaction;
            cmd.ExecuteNonQuery();
        }
    }
}
