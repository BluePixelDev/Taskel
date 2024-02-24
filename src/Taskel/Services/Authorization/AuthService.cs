using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Taskel.Services.Authorization
{
    public class AuthService(SessionService sessionService) : IAuthService
    {
        private static readonly string hashSalt = "DC4U";
        private readonly SessionService sessionService = sessionService;

        public bool Login(string username, string password)
        {
            /*
            string hashPassword = HashString(password);
            DBUserManager dbUserManager = new();
            DBUser? user = dbUserManager.GetUserByName(username);

            if (user == null)
                throw new AuthLoginException(AuthLoginExceptionType.NotFound, "The user could not be found in the database");

            if (user.HashedPassword != hashPassword)
                throw new AuthLoginException(AuthLoginExceptionType.CredentialsMismatch, "The username and password are incorect");

            sessionService.SetSession(user.ID, user.Name);
            */
            return true;
        }

        private static string HashString(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value + hashSalt);
            byte[] hashedData = SHA256.HashData(data);
            string hash = BitConverter
               .ToString(hashedData)
               .Replace("-", "");
            return hash;
        }

        public void Logout()
        {
            sessionService.ClearSeession();
        }

        public bool Register(string username, string password)
        {
            /*
            DBUserManager dbUserManager = new();
            DBUser? userCheck = dbUserManager.GetUserByName(username);

            if (userCheck != null)
                throw new AuthRegisterException(AuthRegisterExceptionType.UserAlreadyExists, "The user with same credentials already exists!");
           
            DBUser user = new(username, password);
            try
            {
                dbUserManager.RegisterUser(user);

            } catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }   
            */
            return true;
        }
    }
}
