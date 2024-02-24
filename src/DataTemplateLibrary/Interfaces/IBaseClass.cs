namespace DataTemplateLibrary.Interfaces
{
    /// <summary>
    /// Interface for objects that work with database to all have ID
    /// </summary>
    /// <creator>Anton Kalashnikov</creator>
    public interface IBaseClass
    {
        int ID { get; set; }
    }
}
