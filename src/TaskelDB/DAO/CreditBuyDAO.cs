using MySqlConnector;
using TaskelDB.Models;
using TaskelDB.Utility;

namespace TaskelDB.DAO
{
    public class CreditBuyDAO : BaseDAO<CreditBuyModel>
    {
        #region QUERIES
        private static readonly string sqlGetCmd = @"
			SELECT
				id,
				amount,
				user_id,				
				bought_date,
			FROM credit_buy_logs
			WHERE id = @id";

        private static readonly string sqlGetAllCmd = @"
			SELECT			
				id,
				amount,
				user_id,				
				bought_date,
			FROM credit_buy_logs";

        private static readonly string sqlCreateCmd = @"
			INSERT INTO credit_buy_logs
			VALUES (@id, @amount, @user_id, @bought_date)";

        private static readonly string sqlDeleteCmd = @"
			DELETE FROM credit_buy_logs
			WHERE id = @id";

        private static readonly string sqlUpdateCmd = @"
			UPDATE credit_buy_logs
			SET id = @id, 
				amount = @amount, 
				user_id = @user_id,
				bought_date = @bought_date,
			WHERE id = @id";
        #endregion

        #region CORE DAO
        /// <summary>
        /// Creates new Transaction entry in the database.
        /// </summary>
        public long Create(CreditBuyModel creditBuy)
        {
            return CreateElement(creditBuy, sqlCreateCmd);
        }

        /// <summary>
        /// Returns transaction from the database with target id.
        /// </summary>
        public CreditBuyModel? Get(long id)
        {
            return GetElement(id, sqlGetCmd);
        }

        /// <summary>
        /// Returns all transactionss from the database.
        /// </summary>
        public List<CreditBuyModel> GetAll()
        {
            return GetElements(sqlGetAllCmd);
        }

        /// <summary>
        /// Update row data of specified Transaction.
        /// </summary>
        public void Update(CreditBuyModel transaction)
        {
            UpdateElement(transaction, sqlUpdateCmd);
        }

        /// <summary>
        /// Deletes transaction with given id from the database
        /// </summary>
        public void Delete(long id)
        {
            DeleteElement(id, sqlDeleteCmd);
        }

        #endregion

        #region MAPPING 
        protected override CreditBuyModel MapSingle(MySqlDataReader reader)
        {
            return new CreditBuyModel()
            {
                ID = reader.GetInt32("id"),
                User_ID = reader.TryGetInt32("user_id"),
                Amount = reader.TryGetInt32("amount"),
                Bought_Date = reader.TryGetDateOnly("bought_date"),
            };
        }
        protected override DBParameters MapToParameters(CreditBuyModel model)
        {
            return new DBParameters()
                .AddParameter("id", model.ID)
                .AddParameter("user_id", model.User_ID)
                .AddParameter("amount", model.Amount)
                .AddParameter("bought_date", model.Bought_Date);
        }
        #endregion


        public void BuyCredits(int userID, int amount)
        {
            if (amount <= 0)
            {
                throw new Exception("The transfer amount can't be equal to zero or be negative!");
            }

            var conn = DBConnection.Instance.GetConnection();
            var transaction = conn.BeginTransaction();

            try
            {
                UserDAO userDAO = new();
                UserDAO.AddCreditsTransaction(conn, transaction, userID, amount);

                CreditBuyModel model = new()
                {
                    User_ID = userID,
                    Bought_Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    Amount = amount
                };

                CreateElementTransaction(model, sqlCreateCmd, conn, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                transaction.Rollback();
            }
        }
    }
}
