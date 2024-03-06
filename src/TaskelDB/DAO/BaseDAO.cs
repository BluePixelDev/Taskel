using MySqlConnector;
using TaskelDB.Interfaces;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    /// <summary>
    /// The base class of classes with DAO pattern.
    /// Contains basic methods for creating, updating, deleting and getting.
    /// </summary>
    public abstract partial class BaseDAO<T> where T : IElement
    {
        /// <summary>
        /// Creates new entry using model.
        /// </summary>
        protected long CreateElement(T element, string sqlCommand)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var data = MapToParameters(element);
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, data);
            var res = cmd.ExecuteScalar();

            //Returns id of the element.
            var identity = DBUtility.GetLastID(conn);
            return identity != null ? Convert.ToInt64(identity) : -1;
        }

        /// <summary>
        /// Updates element using model. <br></br>
        /// <b>NOTE:</b> For this operation the ID of the model must be specified.
        /// </summary>
        protected void UpdateElement(T element, string sqlCommand)
        {
			using var conn = DBConnection.Instance.GetConnection();
			var data = MapToParameters(element);
			using var cmd = DBUtility.CreateCommand(conn, sqlCommand, data);
			var res = cmd.ExecuteScalar();
		}

        /// <summary>
        /// Deletes row with given id.
        /// </summary>
		protected void DeleteElement(long id, string sqlCommand)
#pragma warning restore CA1822 // Označení členů jako statických
		{
			using var conn = DBConnection.Instance.GetConnection();
			DBParameters parameters = new();
			parameters.AddParameter("id", id);

			using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
			cmd.ExecuteScalar();
		}

        /// <summary>
        /// Returns mapped model with specified id.
        /// </summary>
		protected T GetElement(long id, string sqlCommand)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("id", id);

            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            using var reader = cmd.ExecuteReader();
            return MapSingle(reader);
        }

        /// <summary>
        /// Returns all mapped models of SQL query. With additive parameters
        /// </summary>
        protected List<T> GetElements(DBParameters parameters, string sqlCommand)
        {
            using var conn = DBConnection.Instance.GetConnection();
            using var cmd = DBUtility.CreateCommand(conn, sqlCommand, parameters);
            using var reader = cmd.ExecuteReader();
            return MapAll(reader);
        }
        /// <summary>
        /// Returns all mapped models of SQL query.
        /// </summary>
		protected List<T> GetElements(string sqlCommand)
		{
			using var conn = DBConnection.Instance.GetConnection();
			using var cmd = DBUtility.CreateCommand(conn, sqlCommand);
			using var reader = cmd.ExecuteReader();
			return MapAll(reader);
		}


        /// <summary>
        /// Maps all results of SQL query to apropriate data model.
        /// </summary>
        protected List<T> MapAll(MySqlDataReader reader)
        {
			List<T> result = [];
			while (reader.Read())
			{
				result.Add(MapSingle(reader));
			}
			return result;
		}

        /// <summary>
        /// Maps singular entry to a instance of a model.
        /// </summary>
		protected abstract T MapSingle(MySqlDataReader reader);
        /// <summary>
        /// Maps model to paramaters used in queries.
        /// </summary>
        protected abstract DBParameters MapToParameters(T model);
    }
}
