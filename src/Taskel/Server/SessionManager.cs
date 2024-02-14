using System.Security.Cryptography;
using System.Text;

namespace SessionService
{
	/// <summary>
	/// Singleton service that works with session ids.
	/// Cant be connected to blazor directly
	/// </summary>
	/// <creator>Anton Kalashnikov</creator>
	/// <edited>Ondřej Kačírek</edited>
	public class SessionManager
	{
		private static SessionManager? instance;
		public static SessionManager Instance { 
			get 
			{ 
				instance ??= new SessionManager();
				return instance; 
			} 
		}

		private readonly Dictionary<string, int> currentSessions = new();
		private SessionManager()
		{
		}

		/// <summary>
		/// Generates new session and adds it to server.
		/// </summary>
		/// <param name="userId">The id of the user.</param>
		/// <returns>The newly generated session id.</returns>
		public string GenerateNewSession(int userId)
		{
			string sessionId = GenerateSessionId(userId);
            AddSession(sessionId, userId);
			return sessionId;
		}
		
		//Adds a session to the server.
        private void AddSession(string sessionId, int userId)
        {
            currentSessions.Add(sessionId, userId);
        }
		//Removes session from the server.
        public void RemoveSession(string sessionId)
		{
			currentSessions.Remove(sessionId);

        }
		/// <summary>
		/// Returns user id from sessionId
		/// </summary>
		/// <param name="sessionId">The id of the session.</param>
		/// <exception cref="Exception"></exception>
		public int GetUserIdFromSessionId(string sessionId)
		{
			if (SessionExists(sessionId))
			{
				return currentSessions[sessionId];
			}
			return -1;
		}

		/// <summary>
		/// Checks whether session with specific id exist on the server.
		/// </summary>
		public bool SessionExists(string sessionId)
		{
			if (string.IsNullOrEmpty(sessionId))
			{
				return false;
			}

			return currentSessions.ContainsKey(sessionId);
		}

        public static string GenerateSessionId(int userId)
        {
            string datetime = DateTime.Now.ToString();
            string toBeHashedValue = userId + datetime;
            byte[] tmpSource = Encoding.ASCII.GetBytes(toBeHashedValue);
            byte[] tmpHash = MD5.HashData(tmpSource);
            return ByteArrayToString(tmpHash);
        }
        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
