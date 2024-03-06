namespace TaskelDB.Utility
{
    /// <summary>
    /// A wrapper for Database parameters. Under the hood it uses dictionary to store parameter names and their values.
    /// </summary>
    public class DBParameters
    {
        public Dictionary<string, object?> Data { get; private set; } = [];

        /// <summary>
        /// Adds a new parameter to the dictionary.
        /// </summary>
        public DBParameters AddParameter(string name, object? value)
        {
            Data.Add(name, value);
            return this;
        }
        /// <summary>
        /// Removes parameter from the dictionary.
        /// </summary>
        public DBParameters RemoveParameter(string name)
        {
            Data.Remove(name);
            return this;
        }
    }
}
