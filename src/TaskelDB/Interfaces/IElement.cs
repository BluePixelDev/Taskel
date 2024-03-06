namespace TaskelDB.Interfaces
{
    /// <summary>
    /// Base interface for all models utilizing ID.
    /// </summary>
    public interface IElement
    {
        public int ID { get; set; }
    }
}
