using System.Data;
using System.Reflection;
using TaskelDB.Attributes;
using TaskelDB.Interfaces;

namespace TaskelDB.Utility
{
    internal static class DBMapper
    {
        /// <summary>
        /// Maps single entry to a Model.
        /// </summary>
        public static T? MapSingle<T>(this IDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            if (!reader.Read())
                return default;

            T result = Activator.CreateInstance<T>();
            MapProperties(reader, result);
            return result;
        }
        /// <summary>
        /// Maps all entries to a list of Models.
        /// </summary>
        public static List<T> MapAll<T>(this IDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            List<T> resultList = [];
            while (reader.Read())
            {
                T result = Activator.CreateInstance<T>();
                MapProperties(reader, result);
                resultList.Add(result);
            }

            return resultList;
        }
        /// <summary>
        /// Maps all properties to a model.
        /// </summary>
        private static void MapProperties<T>(IDataReader reader, T result)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                try
                {
                    //Checks if user has attribute specifying column name.
                    ColumnNameAttribute? attribute = property.GetCustomAttribute<ColumnNameAttribute>(false);
                    if (attribute != null) propertyName = attribute.ColumnName;

                    string columnName = reader.GetName(reader.GetOrdinal(propertyName));

                    object value = reader[columnName];
                    if (value != DBNull.Value)
                    {
                        property.SetValue(result, Convert.ChangeType(value, property.PropertyType));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error mapping property {propertyName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Creates a dictionary where left side represents column name and right side a value.
        /// </summary>
		public static DBParemeters MapToParameters<T>(T model) where T : IElement
        {
            DBParemeters paremeters = new();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name.ToLower();
                ColumnNameAttribute? attribute = property.GetCustomAttribute<ColumnNameAttribute>(false);
                if (attribute != null) propertyName = attribute.ColumnName;

                paremeters.AddParameter(propertyName, property.GetValue(model, null));
            }
            return paremeters;
        }
    }
}
