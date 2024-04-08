using MySqlConnector;
using TaskelDB.Models.Conversation;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class ConversationDAO : BaseDAO<ConversationModel>
	{
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT
				id,
				name,
				created_at,
			FROM conversations
			WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
			SELECT
				id,
				name,
				created_at,
			FROM users";

        private static readonly string sqlCreateCmd = @"
			INSERT INTO conversations
			VALUES (@id, @name, @created_at)";

        private static readonly string sqlDeleteCmd = @"
			DELETE FROM conversations
			WHERE id = @id";

        private static readonly string sqlUpdateCmd = @"
			UPDATE conversations
			SET id = @id, 
				name = @name, 
				created_at = @created_at,
			WHERE id = @id";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database.
        /// </summary>
        public long Create(ConversationModel conversation)
		{
			try
			{
				return CreateElement(conversation, sqlCreateCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error creating message: {ex.Message}");
			}
			return -1;
		}

		/// <summary>
		/// Returns user from the database with target id.
		/// </summary>
		public ConversationModel? Get(long id)
		{
			try
			{
				return GetElement(id, sqlGetCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error getting message: {ex.Message}");
			}
			return null;
		}

		/// <summary>
		/// Returns all users from the database.
		/// </summary>
		public List<ConversationModel> GetAll()
		{
			try
			{
				return GetElements(sqlGetAllCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error getting all messages: {ex.Message}");
			}

			return [];
		}

		/// <summary>
		/// Update row data of specified User.
		/// </summary>
		public void Update(ConversationModel conversation)
		{
			try
			{
				UpdateElement(conversation, sqlUpdateCmd);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating message with id {conversation.ID}: {ex.Message}");
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
				Console.WriteLine($"Error deleting message with id {id}: {ex.Message}");
			}

		}

        #endregion

        #region MAPPING 
        protected override ConversationModel MapSingle(MySqlDataReader reader)
        {
            return new ConversationModel()
            {
                ID = reader.GetInt32("id"),
				Name = reader.TryGetString("name"),
				Created_At = reader.TryGetDateTime("created_at")
            };
        }
        protected override DBParameters MapToParameters(ConversationModel model)
        {
			return new DBParameters()
				.AddParameter("id", model.ID)
				.AddParameter("name", model.Name)
				.AddParameter("created_at", model.Created_At);
        }
        #endregion
	}
}
