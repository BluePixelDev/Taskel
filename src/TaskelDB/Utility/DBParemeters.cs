namespace TaskelDB.Utility
{
    /// <summary>
    /// A wrapper for Database parameters. Under the hood it uses dictionary to store parameter names and their values.
    /// </summary>
    internal class DBParemeters
    {
        public Dictionary<string, object?> Data { get; private set; } = [];

        /// <summary>
        /// Adds a new parameter to the dictionary.
        /// </summary>
        public DBParemeters AddParameter(string name, object? value)
        {
            Data.Add(name, value);
            return this;
        }
        /// <summary>
        /// Removes parameter from the dictionary.
        /// </summary>
        public DBParemeters RemoveParameter(string name)
        {
            Data.Remove(name);
            return this;
        }
    }
}
