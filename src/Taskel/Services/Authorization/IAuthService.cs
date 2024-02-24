namespace Taskel.Services.Authorization
{
    public interface IAuthService
    {
        bool Login(string username, string password);
        void Logout();
        bool Register(string username, string password);
    }
}
