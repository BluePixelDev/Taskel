using TaskelDB.Interfaces;

namespace TaskelDB.Models.Service
{
    /// <summary>
    /// A data model describbing service category.
    /// </summary>
    public class ServiceCategoryModel : IElement
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }
}
