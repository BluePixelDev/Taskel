using MySqlConnector;
using TaskelDB.Models;
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

        private static readonly string sqlGetByEmail = @"
			SELECT
				users.id,
				name,
				hashedPassword,
				current_credits,
				isAdmin
			FROM users
				LEFT JOIN email_addresses ON email_addresses.user_id = users.id
			WHERE email_address = @email_address";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database.
        /// </summary>
        public long Create(UserModel user)
		{
			try
			{
				CreateElement(user, sqlCreateCmd);
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
			try
			{
				return GetElement(id, sqlGetCmd);
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
			try
			{
				UpdateElement(user, sqlUpdateCmd);
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
			try
			{
				DeleteElement(id, sqlDeleteCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting user with id {id}: {ex.Message}");
			}

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
                HashedPassword = reader.GetString("hashed_password"),
                IsAdmin = reader.GetBoolean("isAdmin")
            };
        }
        protected override DBParameters MapToParameters(UserModel model)
        {
            return new DBParameters()
                .AddParameter("id", model.ID)
                .AddParameter("name", model.Name)
                .AddParameter("current_credits", model.Current_Credits)
                .AddParameter("hashed_Password", model.HashedPassword)
                .AddParameter("isAdmin", model.IsAdmin);
        }
        #endregion

        /// <summary>
        /// Returns user from the database with specified email.
        /// </summary>
        public UserModel? GetByEmail(string emailAddress)
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
				Console.WriteLine($"Error getting user: {ex.Message}");
			}
			return null;
		}
	}
}
