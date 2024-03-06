using MySqlConnector;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class ServiceDAO : BaseDAO<ServiceModel>
    {
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT 
			    id, 
			    user_id, 
			    ser_name, 
                current_price, 
                creation, 
                update, 
                isShown, 
                short_description, 
                long_description, 
                link, isDeleted, 
                category 

            FROM services
            WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
            SELECT 
                id,
                user_id, 
                ser_name, 
                current_price, 
                creation,
                update, 
                isShown, 
                short_description, 
                long_description, 
                link, 
                isDeleted, 
                category 

            FROM services";

        private static readonly string sqlCreateCmd = @"
            INSERT INTO 
                services
            VALUES (
                @id, 
                @user_id, 
                @ser_name, 
                @current_price, 
                @creation, 
                @update, 
                @isShown, 
                @short_description, 
                @long_description,
                @link, 
                @isDeleted, 
                @category)";

        private static readonly string sqlDeleteCmd = @"
             DELETE FROM 
                 services 
             WHERE id = @id";

        private static readonly string sqlUpdateCmd =@"
            UPDATE 
                services 
            SET 
                id= @id, 
                user_id= @user_id, 
                ser_name= @ser_name, 
                current_price= @current_price, 
                creation= @creation, 
                update= @update, isShown= @isShown, 
                short_description= @short_description, 
                long_description= @long_description,link= @link, 
                isDeleted= @isDeleted, 
                category= @category
                current_credits = @current_credits, 
                isAdmin = @isAdmin
            WHERE id = @id";

        private static readonly string sqlGetByUserCmd =@"
            SELECT 
                id, 
                user_id, 
                ser_name, 
                current_price, 
                creation, 
                `update`, 
                isShown, 
                short_description, 
                long_description, 
                link, 
                isDeleted, 
                category 
            FROM 
                services
            WHERE user_id = @user_id";

        private static readonly string sqlGetByPage = @"
			SELECT
				services.id,
				ser_name,
				current_price,
				creation,
				`update`,
				isShown,
				short_description,
				long_description,
				link,
				isDeleted,
				category,

				service_categories.category_name,

				users.id as user_id,
				users.name as user_name
        
			FROM services
				LEFT JOIN users ON services.user_id = users.id
				LEFT JOIN service_categories ON service_categories.id = category
        
			LIMIT @limit
			OFFSET @offset";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new service entry in the database.
        /// </summary>
        public long Create(ServiceModel service)
        {
            try
            {
                return CreateElement(service, sqlCreateCmd);
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
            try
            {
                return GetElement(id, sqlGetCmd);
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
            try
            {
                return GetElements(sqlGetAllCmd);
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
            try
            {
                UpdateElement(service, sqlUpdateCmd);
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
            try
            {
                DeleteElement(id, sqlDeleteCmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting service with id {id}: {ex.Message}");
            }
        }
        #endregion

        #region MAPPING
        protected override ServiceModel MapSingle(MySqlDataReader reader)
        {
            return new ServiceModel()
            {
                ID = reader.GetInt32("id"),
                User_ID = reader.GetInt32("user_id"),
                Ser_Name = reader.GetString("ser_name"),
                Current_Price = reader.GetInt32("current_price"),
                Creation = reader.GetDateTime("creation"),
                Update = reader.GetDateTime("update"),
                IsShown = reader.GetBoolean("isShown"),
                Short_Description = reader.GetString("short_description"),
                Long_Description = reader.GetString("long_description"),
                Link = reader.GetString("link"),
                IsDeleted = reader.GetBoolean("isDeleted"),
                Category = reader.GetInt32("category")
            };
        }
        protected override DBParameters MapToParameters(ServiceModel model)
        {
            return new DBParameters()
                .AddParameter("id", model.ID)
                .AddParameter("user_id", model.User_ID)
                .AddParameter("ser_name", model.Ser_Name)
                .AddParameter("current_price", model.Current_Price)
                .AddParameter("creation", model.Creation)
                .AddParameter("update", model.Update)
                .AddParameter("isShown", model.IsShown)
                .AddParameter("short_description", model.Short_Description)
                .AddParameter("long_description", model.Long_Description)
                .AddParameter("link", model.Link)
                .AddParameter("isDeleted", model.IsDeleted)
                .AddParameter("category", model.Category);
        }

        private static ServiceModel MapSinglePage(MySqlDataReader reader)
        {
            return new ServiceModel()
            {
                ID = reader.GetInt32("id"),
                User_ID = reader.GetInt32("user_id"),
                Ser_Name = reader.GetString("ser_name"),
                Current_Price = reader.GetInt32("current_price"),
                Creation = reader.ReadDateTime("creation"),
                Update = reader.ReadDateTime("update"),
                IsShown = reader.GetBoolean("isShown"),
                Short_Description = reader.ReadString("short_description"),
                Long_Description = reader.ReadString("long_description"),
                Link = reader.ReadString("link"),
                IsDeleted = reader.GetBoolean("isDeleted"),
                Category = reader.GetInt32("category"),

                UserModel = new UserModel()
                {
                    ID = reader.GetInt32("user_id"),
                    Name = reader.GetString("user_name"),
                },

                CategoryModel = new ServiceCategoryModel()
                {
                    ID = reader.GetInt32("category"),
                    Name = reader.GetString("category_name")
                }
            };
        }
        private List<ServiceModel> MapAllPage(MySqlDataReader reader)
        {
            List<ServiceModel> list = [];
            while (reader.Read())
            {
                list.Add(MapSinglePage(reader));
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Returns all services that are owned by specified user from the database.
        /// </summary>
        public List<ServiceModel> GetAllByUser(long userID)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("user_id", userID);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetByUserCmd, parameters);
                using var reader = cmd.ExecuteReader();
                return MapAll(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting services by user: {ex.Message}");
            }

            return [];
        }

        /// <summary>
        /// Returns all services on a specific page.
        /// </summary>
        public List<ServiceModel> GetByPage(int pageNumber, int entriesPerPage)
        {
            using var conn = DBConnection.Instance.GetConnection();
            DBParameters parameters = new();
            parameters.AddParameter("offset", pageNumber * entriesPerPage);
            parameters.AddParameter("limit", entriesPerPage);

            try
            {
                using var cmd = DBUtility.CreateCommand(conn, sqlGetByPage, parameters);
                using var reader = cmd.ExecuteReader();
                return MapAllPage(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting services by page: {ex.Message}");
            }

            return [];
        }
    }
}
