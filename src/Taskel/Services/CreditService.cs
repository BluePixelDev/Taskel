using TaskelDB.DAO;

namespace Taskel.Services
{
    public class CreditService
    {
        public event Action? OnCreditsUpdate;

        public void BuyCredits(int userID, int amount)
        {
            TransactionDAO dao = new();
            dao.AddCreditsToUser(userID, -1, amount);
            OnCreditsUpdate?.Invoke();
        }

        public void SpendCredits(int buyerID, int sellerID, int serviceID, int amount) 
        {
            TransactionDAO dao = new();
            dao.TransferCreditsToUser(buyerID, sellerID, serviceID, amount);
            OnCreditsUpdate?.Invoke();
        }
    }
}
