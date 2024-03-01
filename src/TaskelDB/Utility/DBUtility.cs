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
        public static MySqlCommand CreateCommand(MySqlConnection connection, string sql, DBParemeters parameters)
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
        /// Reads and maps single element.
        /// </summary>
        public static T? ReadAndMapSingle<T>(string sql, DBParemeters paremeters) where T : IElement 
        { 
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = CreateCommand(conn, sql, paremeters);
            using var reader = cmd.ExecuteReader();
            return DBMapper.MapSingle<T>(reader);
        }

        /// <summary>
        /// Reads and maps all elements
        /// </summary>
        public static List<T> ReadAndMapMultiple<T>(string sql, DBParemeters paremeters) where T : IElement
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = CreateCommand(conn, sql, paremeters);
            using var reader = cmd.ExecuteReader();
            return DBMapper.MapAll<T>(reader);
        }
    }
}
