using DataTemplateLibrary.Interfaces;

namespace DataTemplateLibrary.Models
{
    /// <summary>
    /// Data model for user objects from database.
    /// </summary>
    /// <creator>Anton Kalashniko</creator>
    public class DBUser : IBaseClass
    {
        private int id = -1;
        private string name;
        private string hashedPassword;
        private int currentCredits;
        private bool isAdmin = false;

        public DBUser(string name, string hashedPassword, int currentCredits)
        {
            this.name = name;
            this.hashedPassword = hashedPassword;
            this.currentCredits = currentCredits;
        }

        public DBUser(int ID, string name, string hashedPassword, int currentCredits)
        {
            this.ID = ID;
            this.name = name;
            this.hashedPassword = hashedPassword;
            this.currentCredits = currentCredits;
        }

        public DBUser(string name, string hashedPassword)
        {
            this.name = name;
            this.hashedPassword = hashedPassword;
            currentCredits = 0;
        }

        public DBUser(int id, string name, string hashedPassword, int currentCredits, bool isAdmin)
        {
            this.id = id;
            this.name = name;
            this.hashedPassword = hashedPassword;
            this.currentCredits = currentCredits;
            this.isAdmin = isAdmin;
        }

        public int ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string HashedPassword { get => hashedPassword; set => hashedPassword = value; }
        public int CurrentCredits { get => currentCredits; set => currentCredits = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
    }
}
