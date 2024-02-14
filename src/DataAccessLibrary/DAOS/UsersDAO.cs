using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;
using DataTemplateLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{
    /// <summary>
    /// This class is a child of AbstractDAO, it implements the methods and contains the SQL used to obtain data
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    internal class UsersDAO : AbstractDAO<DBUser>, IDAO<DBUser>
    {
        private static string table_n = "Users";
        private String C_CREATE = $"INSERT INTO {table_n} (name, hashedPassword, current_credits, isAdmin) VALUES (@name, @hashedPassword, @current_credits, @isAdmin)";
        private String C_UPDATE = $"UPDATE {table_n} SET name = @name, hashedPassword = @hashedPassword, current_credits = @current_credits, WHERE id = @id";
        private String C_READ_ALL = $"SELECT * FROM {table_n}";
        private String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private String C_GET_BY_NAME = $"SELECT * FROM {table_n} WHERE name = @name";

        public int Create(DBUser element)
        {
            return Create(C_CREATE, element);
        }

        public void Delete(int id)
        {
            Delete(C_DELETE, id);
        }

        public List<DBUser> GetAll()
        {
            return GetAll(C_READ_ALL);
        }

        public DBUser? GetByID(int id)
        {
            return GetByID(C_READ_BY_ID, id);
        }

        public void Save(DBUser element)
        {
            Update(C_UPDATE, element, element.ID);
        }

        public DBUser GetByName(string name)
        {
           return GetByName(C_GET_BY_NAME,"@name",name);
        }

        public DBUser GetByName(DBUser element)
        {
            return GetByName(element.Name);
        }

        protected override DBUser Map(MySqlDataReader reader)
        {
            return new DBUser(
                Convert.ToInt32(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                Convert.ToInt32(reader[3].ToString()),
                ConvertStringToBool(reader[4].ToString())
            );
        }

        protected override List<MySqlParameter> Map(DBUser obj)
        {
            return new List<MySqlParameter>()
            {
                new MySqlParameter("@name",obj.Name),
                new MySqlParameter("@hashedPassword",obj.HashedPassword),
                new MySqlParameter("@current_credits",obj.CurrentCredits),
                new MySqlParameter("@isAdmin",obj.IsAdmin) 
            };
        }

        private bool ConvertStringToBool(string num)
        {
            return (num == "1");
        } 
    }
}
