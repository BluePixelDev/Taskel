using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;
using DataTemplateLibrary.Models;
using MySql.Data.MySqlClient;

namespace DataAccessLibrary.DAOS
{

    internal class TransactionDAO : AbstractDAO<DBTransaction>, IDAO<DBTransaction>
    {
        private static string table_n = "transactions";
        private static string users_table_n = "users";
        // (data) values (@data)
        private static String C_CREATE;
        // data = @data,
        private static String C_UPDATE;
        private static String C_READ_ALL = $"SELECT * FROM {table_n}";
        private static String C_READ_BY_ID = $"SELECT * FROM {table_n} WHERE id = @id";
        private static String C_DELETE = $"DELETE FROM {table_n} WHERE id = @id";
        private static String C_GET_BY_USER_ID = $"SELECT * FROM {table_n} WHERE user_id = @user_id";
        private static String C_GET_BY_SERVICE_ID = $"SELECT * FROM {table_n} WHERE service_id = @service_id";
        private static String C_UPDATE_SENDER = $"UPDATE {users_table_n} SET current_credits = current_credits - @creditsSender WHERE users.id = @idSender;";
        private static String C_UPDATE_RECIEVER = $"UPDATE {users_table_n} SET current_credits = current_credits + @creditsReciever WHERE users.id = @idReciever;";
        private static String C_GET_AMOUNT_OF_BUYS = $"SELECT COUNT(*) FROM {table_n} WHERE service_id = @service_id AND send_date > (SELECT SUBDATE(NOW(),INTERVAL @days DAY))";
        private static String C_GET_TOTAL_MONEY = $"SELECT SUM(cost) FROM {table_n} WHERE service_id = @service_id";
        //select sum(cost) from transactions where service_id = 114;


        private static List<string> atributeList = new List<string>() { "reciever_id", "sender_id", "send_date", "send_time", "cost", "service_id" };


        public TransactionDAO()
        {
            SetSQL(atributeList);
        }
        private void SetSQL(List<string> atributes)
        {
            C_CREATE = SetSQLCreate(atributes,table_n);
            C_UPDATE = SetSQLUpdate(atributes,table_n);
        }
        public void Delete(int id)
        {
            // Delete(C_DELETE, id);
            throw new NotImplementedException("This method is not supported. You cant Delete a transaction");
        }

        public int GetAmountOfBuys(int service_id, int amountOfDays)
        {
            List<MySqlParameter> paramers = new List<MySqlParameter>()
            {
                new MySqlParameter("@service_id",service_id),
                new MySqlParameter("@days",amountOfDays)
            };
            return GetCount(C_GET_AMOUNT_OF_BUYS,paramers);
        }

        public int GetTotalMoneyObtained(int serviceId)
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>() { new MySqlParameter("@service_id", serviceId) };
            return GetCount(C_GET_TOTAL_MONEY,parameters);
        }
        public List<DBTransaction> GetAll()
        {
            return GetAll(C_READ_ALL);
        }

        public DBTransaction? GetByID(int id)
        {
            return GetByID(C_READ_BY_ID, id);
        }

        public void Save(DBTransaction element)
        {
            // Update(C_UPDATE, element, element.ID);
            throw new NotImplementedException("This method is not supported. You cant Update a transaction");
        }

        public List<DBTransaction> GetAllByUserId(int userId)
        {
            return Get(C_GET_BY_USER_ID, new List<MySqlParameter> { new MySqlParameter("@user_id", userId) });
        }

        public bool Create(DBTransaction element, int senderId, int recieverId, int amountRemoving, int amountAdding)
        {
            Dictionary<string, List<MySqlParameter>> SQLAndParams = new Dictionary<string, List<MySqlParameter>>();
            SQLAndParams.Add(C_CREATE, Map(element));
            SQLAndParams.Add(C_UPDATE_SENDER, new List<MySqlParameter>()
            {
                new MySqlParameter("@creditsSender",amountRemoving),
                new MySqlParameter("@idSender",senderId)
            });
            SQLAndParams.Add(C_UPDATE_RECIEVER, new List<MySqlParameter>()
            {
                 new MySqlParameter("@creditsReciever",amountAdding),
                new MySqlParameter("@idReciever",recieverId)
            });
            return TransactionProccess(SQLAndParams);
        }

        public bool Create(DBTransaction element, int senderId, int recieverId, int amount)
        {
            return Create(element,senderId,recieverId,amount,amount);
        }

        protected override DBTransaction Map(MySqlDataReader reader)
        {
            int id = Convert.ToInt32(reader[0]);
            int recieverId = Convert.ToInt32(reader[1]);
            int senderId = Convert.ToInt32(reader[2]);
            DateOnly transactionDate = DateOnly.FromDateTime(DateTime.Parse(reader[3].ToString()));
            TimeOnly transactionTime = TimeOnly.FromDateTime(DateTime.Parse(reader[4].ToString()));
            DateTime dateTime = transactionDate.ToDateTime(transactionTime);
            int amount = Convert.ToInt32(reader[5]);
            int serviceId = Convert.ToInt32(reader[6]);

            return new DBTransaction(id, recieverId, senderId, dateTime, amount, serviceId);
        }

        protected override List<MySqlParameter> Map(DBTransaction obj)
        {
            DateTime dateTime = obj.DateOfTransaction;
            dateTime = dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
            TimeOnly time = TimeOnly.FromTimeSpan(dateTime.TimeOfDay);

            List<MySqlParameter> parameters = new List<MySqlParameter>()
            {
                new MySqlParameter("@reciever_id",obj.RecievingUserId),
                new MySqlParameter("@sender_id",obj.SendingUserId),
                new MySqlParameter("@send_date",DateOnly.FromDateTime(obj.DateOfTransaction).ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@send_time",time.ToString("o",CultureInfo.InvariantCulture)),
                new MySqlParameter("@cost",obj.Amount),
                new MySqlParameter("@service_id",obj.ServiceId)
            };
            return parameters;
        }

        public List<DBTransaction> GetAllByServiceId(int serviceId)
        {
            return Get(C_GET_BY_SERVICE_ID, new List<MySqlParameter> { new MySqlParameter("@service_id", serviceId) });
        }



        [Obsolete]
        public int Create(DBTransaction element)
        {
            throw new NotImplementedException("This creation method is not supported. Use other Create method");
        }
    }
}
