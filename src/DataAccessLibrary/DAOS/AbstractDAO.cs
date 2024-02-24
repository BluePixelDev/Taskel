using System.Data;
using DataTemplateLibrary.Interfaces;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{
    /// <summary>
    /// This abstract class contains logic for obtaining data from SQL Database
    /// </summary>
    /// <typeparam name="T">A class into which ones save the data from database</typeparam>
    /// <creator>Anton Kalashnikov</creator>
    /// <collaborator>Ondrej Kacirek</collaborator>
    internal abstract class AbstractDAO<T> where T : IBaseClass
    {
        public void Update(string SQL, T obj, int id)
        {
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                var parameters = Map(obj);
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                command.Parameters.Add(new MySqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
        public int Create(string SQL, T obj)
        {
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                var parameters = Map(obj);
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                command.ExecuteNonQuery();
                command.CommandText = "Select @@Identity";
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }
        public static void Delete(string SQL, int id)
        {
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                command.Parameters.Add(new MySqlParameter("@id", id));
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        // ==== GET ====
        public List<T> GetAll(string SQL)
        {
            List<T> list = [];
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Map(reader));
                }

            }
            catch
            {
                return list;
            }
            return list;
        }
        public List<T> Get(string SQL, List<MySqlParameter> parameters)
        {
            List<T> list = [];
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(Map(reader));
                }
            }
            catch
            {
                return list;
            }
            return list;
        }
        public T? GetByID(string SQL, int id)
        {
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                command.Parameters.Add(new MySqlParameter("@id", id));

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return Map(reader);
                }
            }
            catch
            {
                return default;
            }
            return default;
        }
        public T? GetByName(string SQL, string parameter_name, string name)
        {
            try
            {
                using var command = CreateCommand();
                command.CommandText = SQL;
                command.Parameters.Add(new MySqlParameter(parameter_name, name));

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return Map(reader);
                }

            }
            catch
            {
                return default;
            }
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLAndParameter">For each sql to be executed add its parameters as a value into a list</param>
        /// <returns>True if transaction was succesfull</returns>
        public static bool TransactionProccess(Dictionary<string, List<MySqlParameter>> SQLAndParameter)
        {
            MySqlConnection? conn = null;
            MySqlCommand? command = new();
            MySqlTransaction transaction;
            conn = DBConnectionSingleton.Instance.GetConnection();
            if (conn.State == ConnectionState.Closed) conn.Open();
            transaction = conn.BeginTransaction();
            command.Connection = conn;
            command.Transaction = transaction;
            try
            {
                foreach (var keyValuePair in SQLAndParameter)
                {
                    command.CommandText = keyValuePair.Key;
                    foreach (var value in keyValuePair.Value)
                    {
                        command.Parameters.Add(value);
                    }
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return false;
            }
        }

        internal static int GetCount(string SQL, List<MySqlParameter> parameters)
        {
            MySqlConnection? conn = null;
            MySqlCommand command;
            conn = DBConnectionSingleton.Instance.GetConnection();
            if (conn.State == ConnectionState.Closed) conn.Open();
            MySqlDataReader? reader = null;
            using (command = conn.CreateCommand())
            {
                command.CommandText = SQL;
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                using (reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader == null) throw new Exception("No data was returned");
                    int? count = null;
                    try
                    {
                        count = Convert.ToInt32(reader[0]);
                    }
                    catch (InvalidCastException e)
                    {
                        count = 0;
                    }
                    return (int)count;
                }
            }
        }

        protected static string SetSQLUpdate(List<string> atributes, string table_n)
        {
            string updateSql = $"UPDATE {table_n} SET ";
            string last = atributes.Last();
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    updateSql += $"{atribute} = @{atribute},";
                }
                else
                {
                    updateSql += $"{atribute} = @{atribute},";
                }
            }
            updateSql += "WHERE id = @id";
            return updateSql;
        }

        protected static string SetSQLCreate(List<string> atributes, string table_n)
        {
            string createSql = $"INSERT INTO {table_n} ";
            createSql += "(";
            string last = atributes.Last();
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    createSql += $"{atribute}";
                }
                else
                {
                    createSql += $"{atribute},";
                }
            }
            createSql += ") values (";
            foreach (var atribute in atributes)
            {
                if (atribute == last)
                {
                    createSql += $"@{atribute}";
                }
                else
                {
                    createSql += $"@{atribute},";
                }
            }
            createSql += ");";
            return createSql;
        }

        protected abstract T Map(MySqlDataReader reader);
        protected abstract List<MySqlParameter> Map(T obj);

        //==== UTILITY ====

        /// <summary>
        /// Checks if connection to Database is open, if not it i'll open the connection.
        /// </summary>
        /// <returns>Created MysqlCommand</returns>
        protected static MySqlCommand CreateCommand()
        {
            MySqlConnection? conn = DBConnectionSingleton.Instance.GetConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn.CreateCommand();
        }
    }

}
