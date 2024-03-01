namespace TaskelDB.Interfaces
{
    /// <summary>
    /// Base interface for DAO classes.
    /// </summary>
    /// <typeparam name="T">The model used by DAO</typeparam>
    internal interface IDAO<T> where T : IElement
    {
        long Create(T entity);
        T? Get(long id);
        List<T> GetAll();
        void Delete(long id);
        void Update(T entity);
    }
}
