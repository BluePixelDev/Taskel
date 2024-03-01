namespace Taskel.Services
{
    /// <summary>
    /// Handles user login sessions.
    /// </summary>
    public class SessionService
    {
        /// <summary>
        /// The id of currently signed in user.
        /// </summary>
        public long UserID { get; private set; } = -1;
        /// <summary>
        /// The username of currently signed in user.
        /// </summary>
        public string Username { get; private set; } = "";
        /// <summary>
        /// Is the user logged in?
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// Event that is called when a session is set. This usually happens when user logs to other account or when they login.
        /// </summary>
        public event Action? OnSessionSet;
        /// <summary>
        /// Event that is called when session is removed. This could for example be when user logs out.
        /// </summary>
        public event Action? OnSessionClear;

        /// <summary>
        /// Sets current session.
        /// </summary>
        /// <param name="userID">The id of the user.</param>
        /// <param name="username">The username of the user.</param>
        public void SetSession(long userID, string username)
        {
            UserID = userID;
            Username = username;
            IsLoggedIn = true;
            OnSessionSet?.Invoke();
        }
        /// <summary>
        /// Clears current session.
        /// </summary>
        public void ClearSeession()
        {
            UserID = -1;
            Username = "";
            IsLoggedIn = false;
            OnSessionClear?.Invoke();
        }
    }
}
