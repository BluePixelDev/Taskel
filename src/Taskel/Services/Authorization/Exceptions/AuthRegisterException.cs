namespace Taskel.Services.Authorization.Exceptions
{
    public enum AuthRegisterExceptionType
    {
        InvalidEmail,
        UserAlreadyExists
    }
    public class AuthRegisterException(AuthRegisterExceptionType exceptionType, string message) : Exception(message)
    {
        public AuthRegisterExceptionType ExceptionType { get; private set; } = exceptionType;

    }
}
