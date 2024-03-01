namespace TaskelDB.Attributes
{
    /// <summary>
    /// An attribute that overrides DBMapper to use specified name for column instead of variable name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class ColumnNameAttribute(string columnName) : Attribute
    {
        public string ColumnName { get; set; } = columnName;
    }
}
