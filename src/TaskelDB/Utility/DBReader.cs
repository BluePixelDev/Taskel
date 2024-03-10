using MySqlConnector;

namespace TaskelDB.Utility
{
    /// <summary>
    /// Utility for reading Data from the query result.
    /// </summary>
    public static class DBReader
    {
        /// <summary>
        /// Attempts to read string. Returns empty string if the field is null.
        /// </summary>
        public static string TryGetString(this MySqlDataReader reader, string name)
        {
            int columnIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(columnIndex) ? reader.GetString(columnIndex) : "";
        }

        /// <summary>
        /// Attempts to read Int16 (short). Returns 0 if the field is null.
        /// </summary>
        public static short TryGetInt16(this MySqlDataReader reader, string name)
        {
            int columnIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(columnIndex) ? reader.GetInt16(columnIndex) : (short)0;
        }

        /// <summary>
        /// Attempts to read Int32 (int). Returns 0 if the field is null.
        /// </summary>
        public static int TryGetInt32(this MySqlDataReader reader, string name)
        {
            int columnIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(columnIndex) ? reader.GetInt32(columnIndex) :0;
        }

        /// <summary>
        /// Attempts to read Int64 (long). Returns 0 if the field is null.
        /// </summary>
        public static long TryGetInt64(this MySqlDataReader reader, string name)
        {
            int columnIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(columnIndex) ? reader.GetInt64(columnIndex) : 0;
        }

        /// <summary>
        /// Attempts to read DateTime. Returns epoch time if the value is null.
        /// </summary>
        public static DateTime TryGetDateTime(this MySqlDataReader reader, string name)
        {
            int columnIndex = reader.GetOrdinal(name);
            return !reader.IsDBNull(columnIndex) ? reader.GetDateTime(columnIndex) : DateTime.UnixEpoch;
        }
    }
}
