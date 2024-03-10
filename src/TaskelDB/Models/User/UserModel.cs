using TaskelDB.Interfaces;

namespace TaskelDB.Models.User
{
    /// <summary>
    /// A data model describbing a User.
    /// </summary>
    public class UserModel : IElement
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public int Current_Credits { get; set; }
        public string HashedPassword { get; set; } = "";
        public bool IsAdmin { get; set; }


        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Credits: {Current_Credits}, HashedPassword: {HashedPassword}, IsAdmin: {IsAdmin}";
        }
    }
}
