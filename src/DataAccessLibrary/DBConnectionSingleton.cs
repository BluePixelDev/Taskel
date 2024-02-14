using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary
{
    /// <summary>
    /// Service used to get connection to database
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public class DBConnectionSingleton
    {
        private static DBConnectionSingleton instance = new DBConnectionSingleton();
        private static MySqlConnection? conn = null;
        private DBConnectionSingleton()
        {
        }
        public static MySqlConnection GetInstance()
        {
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
            try
            {
                if (conn == null)
                {
                    // TODO: Find a way to fix App.config reading
                    SqlConnectionStringBuilder consStringBuilder = new SqlConnectionStringBuilder();
                    consStringBuilder.UserID = "ppraxe";
                    consStringBuilder.Password = "Ppraxe+01";
                    consStringBuilder.InitialCatalog = "praxedb";
                    consStringBuilder.DataSource = "93.99.225.235";
                    consStringBuilder.ConnectTimeout = 10;
                    conn = new MySqlConnection(consStringBuilder.ConnectionString);
                    conn.Open();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Wasn`t able to connect to database, try again later or call an administrator ERR: {e.Message}");
            }
            return conn;
        }
        private static string ReadSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string result = appSettings[key] ?? "Not Found";
            return result;
        }
    }

}

