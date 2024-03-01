using System.Security.Cryptography;
using System.Text;
using Taskel.Services.Authorization.Exceptions;
using TaskelDB.DAO;
using TaskelDB.Models;

namespace Taskel.Services.Authorization
{
    public class AuthService(SessionService sessionService) : IAuthService
    {
        private static readonly string hashSalt = "DC4U";
        private readonly SessionService sessionService = sessionService;

        /// <summary>
        /// Logs in the user and creates a session.
        /// </summary>
        /// <param name="email">The email of target user.</param>
        /// <param name="password">The password of target user.</param>
        /// <returns></returns>
        /// <exception cref="AuthLoginException"></exception>
        public bool Login(string email, string password)
        {
            string hashPassword = HashString(password);

            EmailModel? emailModel = EmailDAO.GetByEmail(email)
                ?? throw new AuthLoginException(AuthLoginExceptionType.EmailNotFound, "The email could not be found in the database");

            UserDAO userDAO = new();
            UserModel? userModel = userDAO.Get(emailModel.User_ID)
                ?? throw new AuthLoginException(AuthLoginExceptionType.UserNotFound, "The user could not be found in the database");

            if (userModel.HashedPassword != hashPassword)
                throw new AuthLoginException(AuthLoginExceptionType.CredentialsMismatch, "The username and password are incorect");

            sessionService.SetSession(userModel.ID, userModel.Name);
            return true;
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public void Logout()
        {
            sessionService.ClearSeession();
        }

        /// <summary>
        /// Registers a new user into the site. Returns exception if user with same email already exists.
        /// If registration goes through user is already registered to the site.
        /// </summary>
        /// <param name="name">The name of registered user.</param>
        /// <param name="email">The email of registered user.</param>
        /// <param name="password">The password of registered user.</param>
        /// <returns></returns>
        /// <exception cref="AuthRegisterException"></exception>
        public bool Register(string name, string email, string password)
        {
            //Checks if email was already used in the database.
            EmailModel? emailModel = EmailDAO.GetByEmail(email);
            if(emailModel != null)
            {
                throw new AuthRegisterException(AuthRegisterExceptionType.UserAlreadyExists, $"User with this email, {email}, already exists!");
            }

            //Creates new user.
            UserDAO userDAO = new();
            long userID = userDAO.Create(new UserModel()
            {
                Name = name,
                HashedPassword = HashString(password)
            });

            //Creates new email.
            EmailDAO emailDAO = new();
            emailDAO.Create(new EmailModel()
            {
                User_ID = userID,
                Email_Address = email
            });

            sessionService.SetSession(userID, name);
            return true;
        }

        /// <summary>
        /// Hashes string using SHA256 and a salt variation.
        /// </summary>
        private static string HashString(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value + hashSalt);
            byte[] hashedData = SHA256.HashData(data);
            string hash = BitConverter
               .ToString(hashedData)
               .Replace("-", "");
            return hash;
        }
    }
}
