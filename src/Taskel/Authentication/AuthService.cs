using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Taskel.Authentication.Exceptions;
using TaskelDB.DAO;
using TaskelDB.Models.User;

namespace Taskel.Authentication
{
    public class AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        private static readonly string hashSalt = "TSKL";
        private static readonly string emailPattern = @"^(?!.*@.*@)([a-z0-9]|-|\+|_|~|.)+@([a-z0-9]|-|_)+\.([a-z0-9]){2,}";
        private readonly TaskelAuthenticationStateProvider authProvider = (TaskelAuthenticationStateProvider)authenticationStateProvider;

        /// <summary>
        /// Logs in the user and creates a session.
        /// </summary>
        /// <param name="email">The email of target user.</param>
        /// <param name="password">The password of target user.</param>
        /// <returns></returns>
        /// <exception cref="AuthLoginException"></exception>
        public async Task<bool> Login(string email, string password)
        {
            if (Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                EmailDAO emailDAO = new();
                EmailModel? emailModel = emailDAO.GetEmailByAddress(email.ToLower())
                    ?? throw new AuthLoginException(AuthLoginExceptionType.EmailNotFound, "The email could not be found in the database");

                UserDAO userDAO = new();
                UserModel? userModel = userDAO.Get(emailModel.User_ID)
                    ?? throw new AuthLoginException(AuthLoginExceptionType.UserNotFound, "The user could not be found in the database");

                string hashPassword = HashString(password);
                if (userModel.HashedPassword != hashPassword)
                    throw new AuthLoginException(AuthLoginExceptionType.CredentialsMismatch, "The username and password are incorect");

                await authProvider.UpdateAuthenticationState(new UserSession()
                {
                    UserID = userModel.ID,
                    Username = userModel.Name,
                });
            }
            else
            {
                throw new AuthLoginException(AuthLoginExceptionType.InvalidEmail, "The email is invalid!");
            }

            return true;
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public async Task Logout()
        {
            await authProvider.UpdateAuthenticationState(null);
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
        public async Task<bool> Register(string name, string email, string password, bool autoLogin = true)
        {
            if (Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                //Checks if email was already used in the database.
                EmailDAO emailDAO = new();
                EmailModel? emailModel = emailDAO.GetEmailByAddress(email);
                if (emailModel != null)
                {
                    throw new AuthRegisterException(AuthRegisterExceptionType.UserAlreadyExists, $"User with this email, {email}, already exists!");
                }

                //Creates new user.
                UserDAO userDAO = new();
                long userID = userDAO.Create(new UserModel()
                {
                    Name = name,
                    HashedPassword = HashString(password),
                });

                //Creates new email.
                emailDAO.Create(new EmailModel()
                {
                    User_ID = userID,
                    Email_Address = email.ToLower()
                });

                if (autoLogin)
                    await Login(email, password);

                return true;
            }
            else
            {
                throw new AuthRegisterException(AuthRegisterExceptionType.InvalidEmail, "The email is invalid!");
            }
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
