using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Interfaces;

namespace DataTemplateLibrary.Models
{
    public class DBTransaction : IBaseClass
    {
        private int id = -1;
        private int recievingUserId;
        private int sendingUserId;
        private DateTime dateOfTransaction;
        private int amount;
        private int? serviceId;

        public DBTransaction(int recievingUserId, int sendingUserId, DateTime dateOfTransaction, int amount, int serviceId)
        {
            this.recievingUserId = recievingUserId;
            this.sendingUserId = sendingUserId;
            this.dateOfTransaction = dateOfTransaction;
            this.amount = amount;
            this.serviceId = serviceId;
        }
        public DBTransaction(int iD, int recievingUserId, int sendingUserId, DateTime dateOfTransaction, int amount, int serviceId)
        {
            ID = iD;
            RecievingUserId = recievingUserId;
            SendingUserId = sendingUserId;
            DateOfTransaction = dateOfTransaction;
            Amount = amount;
            ServiceId = serviceId;
        }
        public DBTransaction(int recievingUserId, int sendingUserId, DateTime dateOfTransaction, int amount, int? serviceId)
        {
            RecievingUserId = recievingUserId;
            SendingUserId = sendingUserId;
            DateOfTransaction = dateOfTransaction;
            Amount = amount;
            ServiceId = serviceId;
        }

        public int ID { get => id; set => id = value; }
        public int RecievingUserId { get => recievingUserId; set => recievingUserId = value; }
        public int SendingUserId { get => sendingUserId; set => sendingUserId = value; }
        public DateTime DateOfTransaction { get => dateOfTransaction; set => dateOfTransaction = value; }
        public int Amount { get => amount; set => amount = value; }
        public int? ServiceId { get => serviceId; set => serviceId = value; }
    }
}
