using MySqlConnector;
using TaskelDB.Interfaces;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    internal abstract class BaseDAO<T> where T : IElement
    {
        protected long Create(string sqlCommand, T element)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var data = MapToParameters(element);
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, data);
            var res = cmd.ExecuteScalar();

            //Returns id of the element.
            var identity = DBUtility.GetLastID(conn);
            return identity != null ? Convert.ToInt64(identity) : -1;
        }

        protected T GetElement(long id, string sqlCommand)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            using var reader = cmd.ExecuteReader();
            return MapSingle(reader);
        }

        protected T GetElements(long id, string sqlCommand)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            using var reader = cmd.ExecuteReader();
            return MapSingle(reader);
        }

        protected abstract T MapSingle(MySqlDataReader reader);
        protected abstract List<T> MapAll(MySqlDataReader reader);
        protected abstract DBParameters MapToParameters(T model);
    }
}
