namespace TaskelDB.Interfaces
{
    /// <summary>
    /// Base interface for all models utilizing ID.
    /// </summary>
    internal interface IElement
    {
        public int ID { get; set; }
    }
}
