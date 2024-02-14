using CookieService;
using DataTemplateLibrary.Models;
using MySqlX.XDevAPI.Common;
using ServerManagement;
using SessionService;
using System.Security.Cryptography;
using System.Text;

namespace LoginService
{
    public class LoginManager
    {
        protected CookieManager? CookieManager { get; set; }
        protected ServerManager? ServerManager { get; set; }

        /// <summary>
        /// This event is invoked when user is logged in.
        /// This includes the first moment the user enters the website.
        /// </summary>
        public event Action? OnLogin;
        /// <summary>
        /// This event is invoked when a new user signs up.
        /// </summary>
        public event Action<DBUser>? OnSignup;
        /// <summary>
        /// This event is being invoked whenever user logs out.
        /// </summary>
        public event Action? OnLogout;
        /// <summary>
        /// This event is being invoked whenever user logs in.
        /// </summary>
        public event Action? OnUpdate;

        public string SessionID { get; private set; } = "";
        public int UserID { get => GetUserID(); }
        public bool LoggedIn { get => IsLoggedIn(); }
        private static readonly string salt = "DC4U";

        public LoginManager(CookieManager cookieManager, ServerManager serverManager)
        {
            CookieManager = cookieManager;
            ServerManager = serverManager;
        }

        /// <summary>
        /// Fetches all the data required.
        /// </summary>
        public async Task Fetch()
        {
            if (CookieManager != null)
            {
                string sessionID = await CookieManager.GetSessionCookieString("sessionID");
                if (CheckIfSessionExists(sessionID))
                {
                    SessionID = sessionID;
                    OnLogin?.Invoke();
                }
                else
                {
                    SetCurrentSession("");
                }
            }
            OnUpdate?.Invoke();
            return;
        }

        /// <summary>
        /// Logs in the user usig it's credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="LoginSignupException"></exception>
        public void Login(string username, string password)
        {
            if (IsLoggedIn())
            {
                Logout();
            }
            if (ServerManager == null)
            {
                throw new LoginSignupException("The ServerManager is null");
            }

            try
            {
                DBUser user = new(username, HashString(password));
                string sessionID = ServerManager.LogUserInCreateSession(user);
                SetCurrentSession(sessionID);
            }
            catch(Exception e)
            {
                throw new LoginSignupException(e.Message);
            }
            OnLogin?.Invoke();
            OnUpdate?.Invoke();
        }

        /// <summary>
        /// Using username and password signs up the user into the website.
        /// </summary>
        /// <param name="username">The name of the user we want to signup.</param>
        /// <param name="password">The non-hashed password of the user. (Hashing is done automatically)</param>
        /// <exception cref="LoginSignupException"></exception>
        public DBUser Signup(string username, string password)
        {
            DBUser? user = new(username, HashString(password));
            DBUser? newUser;

            //Checks whether the ServerManager was null.
            if(ServerManager == null)
            {
                throw new LoginSignupException("The server manager was null");
            }

            //Tries to signup the user into the database.
            try{
                newUser = ServerManager.SingUpUser(user);
            }
            catch(Exception e)
            {
                throw new LoginSignupException(e.Message);
            }

            //Checks if the new user result is not null.
            if (newUser == null)
            {              
                throw new LoginSignupException("The new user is null");
            }

            //Automatically logs in the user.
            OnSignup?.Invoke(newUser);
			Login(username, password);
			return newUser;        
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public void Logout()
        {
            SessionManager.Instance.RemoveSession(SessionID);
            SetCurrentSession("");
            OnLogout?.Invoke();
            OnUpdate?.Invoke();
        }
    
        private int GetUserID()
        {
            try
            {
                return SessionManager.Instance.GetUserIdFromSessionId(SessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }
        private bool IsLoggedIn()
        {
            bool loginStatus = false;
            if (CookieManager != null)
            {
                if (CheckIfSessionExists(SessionID))
                {
                    loginStatus = true;
                }
            }
            return loginStatus;
        }

        private static string HashString(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value + salt);
            byte[] hashedData = SHA256.Create().ComputeHash(data);
            string hash = BitConverter
               .ToString(hashedData)
               .Replace("-","");
            return hash;
        }
        private static bool CheckIfSessionExists(string sessionId)
        {
            try
            {
                if (SessionManager.Instance.SessionExists(sessionId))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
        private void SetCurrentSession(string sessionID)
        {
            SessionID = sessionID;
            CookieManager?.SetSessionCookie("sessionID", sessionID);
        }
    }
}