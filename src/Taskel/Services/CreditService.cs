using TaskelDB.DAO;

namespace Taskel.Services
{
    public class CreditService
    {
        public event Action? OnCreditsUpdate;

        public void BuyCredits(int userID, int amount)
        {
            CreditBuyDAO dao = new();
            dao.BuyCredits(userID, amount);
            OnCreditsUpdate?.Invoke();
        }

        public void BuyService(int buyerID, int sellerID, int serviceID, int amount) 
        {
            TransactionDAO dao = new();
            dao.TransferCreditsToUser(buyerID, sellerID, serviceID, amount);
            OnCreditsUpdate?.Invoke();
        }
    }
}
