using MySql.Data.MySqlClient;

namespace DataAccessLibrary
{
    /// <summary>
    /// Service used to get connection to database
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public class DBConnectionSingleton
    {
        private static readonly DBConnectionSingleton instance = new();
        public static DBConnectionSingleton Instance { get => instance; }

        private MySqlConnection? connection = null;

        public string ConnectionString { get; set; } = "";

        private DBConnectionSingleton() { }
        public MySqlConnection GetConnection()
        {
            if (connection != null)
            {
                //Closes connection if it is open.
                if (connection.State == System.Data.ConnectionState.Open) 
                    connection.Close();
            }
            else
            {
                //Creates new connection if there is none.
                connection = CreateConnection();
            }
            return connection;
        }

        public MySqlConnection CreateConnection()
        {
            MySqlConnection connection = new(ConnectionString);
            try
            {
                connection.Open();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return connection;
        }     
    }

}

