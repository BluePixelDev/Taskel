using MySqlConnector;
using TaskelDB.Models.Conversation;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class MessagesDAO : BaseDAO<MessageModel>
    {
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT
				id,
				conversation_id,
				sender_id,
				body,
				created_at
			FROM messages
			WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
			SELECT
				id,
				conversation_id,
				sender_id,
				body,
				created_at
			FROM messages";

        private static readonly string sqlCreateCmd = @"
			INSERT INTO messages
			VALUES (@id, @conversation_id, @sender_id, @body, @created_id)";

        private static readonly string sqlDeleteCmd = @"
			DELETE FROM users
			WHERE id = @id";

        private static readonly string sqlUpdateCmd = @"
			UPDATE messages
			SET id = @id, 
				conversation_id = @conversation_id, 
				sender_id = @sender_id,
				body = @body,
				created_at = @created_at
			WHERE id = @id";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new User entry in the database.
        /// </summary>
        public long Create(MessageModel message)
        {
            try
            {
                return CreateElement(message, sqlCreateCmd);
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
        public MessageModel? Get(long id)
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
        public List<MessageModel> GetAll()
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
        public void Update(MessageModel message)
        {
            try
            {
                UpdateElement(message, sqlUpdateCmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating message with id {message.ID}: {ex.Message}");
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
        protected override MessageModel MapSingle(MySqlDataReader reader)
        {
            return new MessageModel()
            {
                ID = reader.GetInt32("id"),
                Conversation_ID = reader.GetInt32("conversation_id"),
                Sender_ID = reader.GetInt32("sender_id"),
                Body = reader.TryGetString("body"),
                Created_At = reader.TryGetDateTime("created_at")
            };
        }
        protected override DBParameters MapToParameters(MessageModel model)
        {
            return new DBParameters()
                .AddParameter("id", model.ID)
                .AddParameter("conversation_id", model.Conversation_ID)
                .AddParameter("sender_id", model.Sender_ID)
                .AddParameter("body", model.Body)
                .AddParameter("created_at", model.Created_At);
        }
        #endregion
    }
}
