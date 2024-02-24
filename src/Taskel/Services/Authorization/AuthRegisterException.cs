namespace Taskel.Services.Authorization
{
    public enum AuthRegisterExceptionType
    {
        UserAlreadyExists,
        DatabaseError
    }
    public class AuthRegisterException(AuthRegisterExceptionType exceptionType, string message) : Exception(message)
    {
        public AuthRegisterExceptionType ExceptionType { get; private set; } = exceptionType;

    }
}
