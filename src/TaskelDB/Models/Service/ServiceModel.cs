using TaskelDB.Interfaces;
using TaskelDB.Models.User;

namespace TaskelDB.Models.Service
{
    /// <summary>
    /// A data model describbing a service.
    /// </summary>
    public class ServiceModel : IElement
    {
        public int ID { get; set; }
        public int User_ID { get; set; }
        public string Ser_Name { get; set; } = "";
        public int Current_Price { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Update { get; set; }
        public bool IsShown { get; set; }
        public string Short_Description { get; set; } = "";
        public string Long_Description { get; set; } = "";
        public string Link { get; set; } = "";
        public bool IsDeleted { get; set; }
        public int Category { get; set; }

        public UserModel? UserModel { get; set; }
        public ServiceCategoryModel? CategoryModel { get; set; }

        public override string ToString()
        {
            return $"ID: {ID}, User_ID: {User_ID}, Ser_Name: {Ser_Name}, Current_Price: {Current_Price}, " +
                   $"Creation: {Creation}, Update: {Update}, IsShown: {IsShown}, " +
                   $"Short_Description: {Short_Description}, Long_Description: {Long_Description}, Link: {Link}, " +
                   $"IsDeleted: {IsDeleted}, Category: {Category}";
        }
    }
}
