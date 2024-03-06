using MySqlConnector;

namespace TaskelDB.Utility
{
    [Obsolete("Please do not use yet as it is not fully developed.")]
    internal partial class DAOUtility
    {
        /// <summary>
        /// Executes given code and returns last inserted id.
        /// </summary>
        /// <param name="sqlCommand">The SQL command to execute.</param>
        /// <param name="parameters">The paramaeters of the SQL query.</param>
        public static long CreateElement(string sqlCommand, DBParameters parameters)
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            cmd.ExecuteScalar();
            var identity = DBUtility.GetLastID(conn);
            return identity != null ? Convert.ToInt64(identity) : -1;
        }

        /// <summary>
        /// Executes SQL query and returns reader.
        /// </summary>
        /// <param name="sqlCommand">The SQL querry to execute.</param>
        /// <param name="parameters">The parameters of the query.</param>
        public static MySqlDataReader GetElements(string sqlCommand, DBParameters parameters)
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            return cmd.ExecuteReader();
        }

        public static void DeleteElements(string sqlCommand, DBParameters parameters)
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            cmd.ExecuteScalar();
        }

        public static void UpdateElements(string sqlCommand, DBParameters parameters)
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            cmd.ExecuteScalar();
        }
    }
}
