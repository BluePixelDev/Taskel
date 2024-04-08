namespace Taskel.Authentication
{
    /// <summary>
    /// Handles user login sessions.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// The id of currently signed in user.
        /// </summary>
        public long UserID { get; set; } = -1;
        /// <summary>
        /// The username of currently signed in user.
        /// </summary>
        public string Username { get; set; } = "";
    }
}
