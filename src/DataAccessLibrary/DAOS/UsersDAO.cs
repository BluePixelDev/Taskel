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
        private readonly static string table_n = "Users";
        private readonly string C_CREATE =
            $"INSERT INTO " +
            $"{table_n} " +
            $"(name, hashedPassword, current_credits, isAdmin) " +
            $"VALUES " +
            $"(@name, @hashedPassword, @current_credits, @isAdmin)";

        private readonly string C_UPDATE =
            $"UPDATE " +
            $"{table_n} " +
            $"SET " +
            $"name = @name, " +
            $"hashedPassword = @hashedPassword, " +
            $"current_credits = @current_credits, " +
            $"WHERE id = @id";

        private readonly string C_READ_ALL = 
            $"SELECT " +
            $"* " +
            $"FROM " +
            $"{table_n}";

        private readonly string C_READ_BY_ID = 
            $"SELECT " +
            $"* " +
            $"FROM " +
            $"{table_n} " +
            $"WHERE " +
            $"id = @id";

        private readonly string C_DELETE = 
            $"DELETE FROM " +
            $"{table_n} " +
            $"WHERE " +
            $"id = @id";

        private readonly string C_GET_BY_NAME = 
            $"SELECT " +
            $"* " +
            $"FROM " +
            $"{table_n} " +
            $"WHERE " +
            $"name = @name";

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

        public DBUser? GetByName(string name)
        {
            return GetByName(C_GET_BY_NAME, "@name", name);
        }

        public DBUser? GetByName(DBUser element)
        {
            return GetByName(element.Name);
        }

        protected override DBUser Map(MySqlDataReader reader)
        {
            return new DBUser(
                Convert.ToInt32(reader[0].ToString()),
                reader[1].ToString() ?? "",
                reader[2].ToString() ?? "",
                Convert.ToInt32(reader[3].ToString()),
                ConvertStringToBool(reader[4].ToString() ?? "0")
            );
        }

        protected override List<MySqlParameter> Map(DBUser obj)
        {
            return
            [
                new ("@name",obj.Name),
                new ("@hashedPassword",obj.HashedPassword),
                new ("@current_credits",obj.CurrentCredits),
                new ("@isAdmin",obj.IsAdmin)
            ];
        }

        private static bool ConvertStringToBool(string num)
        {
            return (num == "1");
        }
    }
}
