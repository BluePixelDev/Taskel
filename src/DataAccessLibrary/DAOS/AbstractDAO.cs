using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using DataTemplateLibrary.Interfaces;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace DataAccessLibrary.DAOS
{
    /// <summary>
    /// This abstract class contains logic for obtaining data from SQL Database
    /// </summary>
    /// <typeparam name="T">A class into which ones save the data from database</typeparam>
    /// <creator>Anton Kalashnikov</creator>>
    internal abstract class AbstractDAO<T> where T : IBaseClass
    {
        public void Update(String SQL, T obj, int id)
        {
            MySqlConnection? conn = null;
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    var parameters = Map(obj);
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    command.Parameters.Add(new MySqlParameter("@id", id));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int Create(String SQL, T obj)
        {
            MySqlConnection? conn = null;
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
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
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public void Delete(String SQL, int id)
        {
            MySqlConnection? conn = null;
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    command.Parameters.Add(new MySqlParameter("@id", id));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<T> GetAll(String SQL)
        {
            MySqlConnection? conn = null;
            MySqlDataReader? reader = null;
            List<T> list = new List<T>();
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Map(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return list;
            }
            return list;
        }
        public List<T> Get(String SQL, List<MySqlParameter> parameters)
        {
            MySqlConnection? conn = null;
            MySqlDataReader? reader = null;
            List<T> list = new List<T>();
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Map(reader));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return list;
            }
            return list;
        }
        public T? GetByID(String SQL, int id)
        {
            MySqlConnection? conn = null;
            MySqlDataReader? reader = null;
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    command.Parameters.Add(new MySqlParameter("@id", id));
                    using (reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            return Map(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
            return default(T);
        }
        public List<T> GetByConnectingID(String SQL, int id, String tag)
        {
            MySqlConnection? conn = null;
            MySqlDataReader? reader = null;
            List<T> list = new List<T>();
            try
            {
                conn = DBConnectionSingleton.GetInstance();

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    command.Parameters.Add(new MySqlParameter(tag, id));
                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(Map(reader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return list;
        }
        public T GetByName(String SQL, string parameter_name, string name)
        {
            MySqlConnection? conn = null;
            MySqlDataReader? reader = null;
            try
            {
                conn = DBConnectionSingleton.GetInstance();
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL;
                    command.Parameters.Add(new MySqlParameter(parameter_name, name));
                    using (reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            return Map(reader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLAndParameter">For each sql to be executed add its parameters as a value into a list</param>
        /// <returns>True if transaction was succesfull</returns>
        public bool TransactionProccess(Dictionary<string, List<MySqlParameter>> SQLAndParameter)
        {
            MySqlConnection? conn = null;
            MySqlCommand? command = new MySqlCommand();
            MySqlTransaction transaction;
            conn = DBConnectionSingleton.GetInstance();
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

        internal int GetCount(string SQL, List<MySqlParameter> parameters)
        {
            MySqlConnection? conn = null;
            MySqlCommand command;
            conn = DBConnectionSingleton.GetInstance();
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
                    } catch (InvalidCastException e)
                    {
                        count = 0;
                    }
                    return (int)count;
                }
            }
        }

        protected string SetSQLUpdate(List<string> atributes, string table_n)
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

        protected string SetSQLCreate(List<string> atributes, string table_n)
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
    }

}
