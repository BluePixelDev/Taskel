using MySqlConnector;

namespace TaskelDB
{
    /// <summary>
    /// Class handling database connection.
    /// </summary>
    public class DBConnection
    {
        private static DBConnection? instance;
        public static DBConnection Instance { get => instance ??= new(); }

        private MySqlConnection? connection;
        public string ConnectionString { get; set; } = "";

        /// <summary>
        /// Creates or retrieves database connection.
        /// </summary>
        public MySqlConnection GetConnection()
        {
            if (connection == null || connection.State == System.Data.ConnectionState.Closed)
            {
                connection?.Dispose();
                connection = new MySqlConnection(ConnectionString);
                connection.Open();
            }

            return connection;
        }
    }
}
