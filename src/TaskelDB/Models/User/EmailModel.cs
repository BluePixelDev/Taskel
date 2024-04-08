using TaskelDB.Interfaces;

namespace TaskelDB.Models.User
{
    /// <summary>
    /// A data model describbing an Email.
    /// </summary>
    public class EmailModel : IElement
    {
        public int ID { get; set; }
        public long User_ID { get; set; }
        public string Email_Address { get; set; } = "";

        public UserModel? UserModel { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, User_ID: {User_ID}, Email_Address: {Email_Address}";
        }
    }
}
