namespace Taskel.Services
{
    public class SessionService
    {
        /// <summary>
        /// The id of currently signed in user.
        /// </summary>
        public int UserID { get; private set; } = -1;
        /// <summary>
        /// The username of currently signed in user.
        /// </summary>
        public string Username { get; private set; } = "";
        /// Is the user logged in?
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// Sets current session.
        /// </summary>
        /// <param name="userID">The id of the user.</param>
        /// <param name="username">The username of the user.</param>
        public void SetSession(int userID, string username)
        {
            UserID = userID;
            Username = username;
            IsLoggedIn = true;
        }
        /// <summary>
        /// Clears current session.
        /// </summary>
        public void ClearSeession()
        {
            UserID = -1;
            Username = "";
            IsLoggedIn = false;
        }
    }
}
