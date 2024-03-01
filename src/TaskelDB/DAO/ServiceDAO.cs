using TaskelDB.Interfaces;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class ServiceDAO : IDAO<ServiceModel>
    {
        private static readonly string sqlGetCmd =
            "SELECT id, user_id, ser_name, current_price, creation, update, isShown, short_description, long_description, link, isDeleted, category " +
            "FROM services " +
            "WHERE id = @id";

        private static readonly string sqlGetAllCmd =
            "SELECT id, user_id, ser_name, current_price, creation, update, isShown, short_description, long_description, link, isDeleted, category " +
            "FROM services";

        private static readonly string sqlCreateCmd =
            "INSERT INTO services " +
            "VALUES (@id, @user_id, @ser_name, @current_price, @creation, @update, @isShown, @short_description, @long_description, @link, @isDeleted, @category)";

        private static readonly string sqlDeleteCmd =
            "DELETE FROM services " +
            "WHERE id = @id";

        private static readonly string sqlUpdateCmd =
            "UPDATE services " +
            "SET id= @id, user_id= @user_id, ser_name= @ser_name, current_price= @current_price, creation= @creation, update= @update, isShown= @isShown, short_description= @short_description, long_description= @long_description,link= @link, isDeleted= @isDeleted, category= @category " +
            "current_credits = @current_credits, isAdmin = @isAdmin " +
            "WHERE id = @id";

        private static readonly string sqlGetByUserCmd =
            "SELECT id, user_id, ser_name, current_price, creation, `update`, isShown, short_description, long_description, link, isDeleted, category " +
            "FROM services " +
            "WHERE user_id = @user_id";

        #region CORE DAO
        /// <summary>
        /// Creates new service entry in the database.
        /// </summary>
        public long Create(ServiceModel service)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var parameters = DBMapper.MapToParameters(service);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlCreateCmd, parameters);
                cmd.ExecuteScalar();
                var identity = DBUtility.GetLastID(conn);
                return identity != null ? Convert.ToInt64(identity) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating service: {ex.Message}");
            }
            return -1;
        }

        /// <summary>
        /// Returns service from the database with target id.
        /// </summary>
        public ServiceModel? Get(long id)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParemeters parameters = new();
            parameters.AddParameter("id", id);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return reader.MapSingle<ServiceModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting service: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Returns all services from the database.
        /// </summary>
        public List<ServiceModel> GetAll()
        {
            using var conn = DBConnection.Instance.GetConnection();

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetAllCmd);
                using var reader = cmd.ExecuteReader();
                return reader.MapAll<ServiceModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all services: {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Updates row data of specified service.
        /// </summary>
        public void Update(ServiceModel service)
        {
            using var conn = DBConnection.Instance.GetConnection();
            var data = DBMapper.MapToParameters(service);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlUpdateCmd, data);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating service with id {service.ID}: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes service with given id from the database
        /// </summary>
        public void Delete(long id)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParemeters parameters = new();
            parameters.AddParameter("id", id);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlDeleteCmd, parameters);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting service with id {id}: {ex.Message}");
            }

        }
        #endregion

        /// <summary>
        /// Returns all services that are owned by specified user from the database.
        /// </summary>
        public static List<ServiceModel> GetAllByUser(long userID)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParemeters parameters = new();
            parameters.AddParameter("user_id", userID);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetByUserCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return reader.MapAll<ServiceModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting services by user: {ex.Message}");
            }

            return [];
        }
    }
}
