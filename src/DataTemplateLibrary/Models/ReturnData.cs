namespace DataTemplateLibrary.Models
{
    /// <summary>
    /// Object for saving and returning multiple data types
    /// </summary>
    /// <typeparam name="T">Any key value</typeparam>
    /// <typeparam name="K">Any additional message</typeparam>
    /// <creator>Anton Kalashnikov</creator>
    public class ReturnData<T,K>
    {
        private T result;
        private K message;

        public ReturnData(T result, K message)
        {
            Result = result;
            Message = message;
        }

        public T Result { get => result; set => result = value; }
        public K Message { get => message; set => message = value; }

    }
}