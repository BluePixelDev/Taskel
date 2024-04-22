using TaskelDB.Interfaces;

namespace TaskelDB.Models
{
    public class CreditBuyModel: IElement
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public int User_ID { get; set; }
        public DateOnly Bought_Date { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, Amount: {Amount}, User ID: {User_ID}, Bought Date: {Bought_Date}";
        }
    }
}
