using TaskelDB.Interfaces;

namespace TaskelDB.Models
{
    /// <summary>
    /// A data model describbing a Transaction.
    /// </summary>
    public class TransactionModel : IElement
    {
        public int ID { get; set; }
        public int Receiver_ID { get; set; }
        public int Sender_ID { get; set; }
        public DateOnly Send_Date { get; set; }
        public TimeOnly Send_Time { get; set; }
        public int Cost { get; set; }
        public int Service_ID { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, Receiver_ID: {Receiver_ID}, Sender_ID: {Sender_ID}, Send_Date: {Send_Date}, Send_Time: {Send_Time}, Cost: {Cost}, Service_ID: {Service_ID}";
        }
    }
}
