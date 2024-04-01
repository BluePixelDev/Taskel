using MySqlConnector;
using TaskelDB.Interfaces;

namespace TaskelDB.Utility
{
    /// <summary>
    /// Utility methods for the database integration.
    /// </summary>
    internal static class DBUtility
    {
        /// <summary>
        /// Creates new command from connection and inserts specified parameters into it.
        /// </summary>
        public static MySqlCommand CreateCommand(MySqlConnection connection, string sql, DBParameters parameters)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            foreach (var parameter in parameters.Data)
            {
                MySqlParameter mySqlParameter = new()
                {
                    ParameterName = parameter.Key,
                    Value = parameter.Value
                };

                cmd.Parameters.Add(mySqlParameter);
            }

            return cmd;
        }

        /// <summary>
        /// Creates new command from connection and inserts sql command.
        /// </summary>
        public static MySqlCommand CreateCommand(MySqlConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        /// <summary>
        /// Returns last inserted ID.
        /// </summary>
        public static object? GetLastID(MySqlConnection connection)
        {
            using var cmd = CreateCommand(connection, "SELECT LAST_INSERT_ID()");
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Returns last inserted ID.
        /// </summary>
        public static object? GetLastIDTransaction(MySqlConnection connection, MySqlTransaction transaction)
        {
            using var cmd = CreateCommand(connection, "SELECT LAST_INSERT_ID()");
            cmd.Transaction = transaction;
            return cmd.ExecuteScalar();
        }
    }
}
