namespace Taskel.Services.Authorization.Exceptions
{
    public enum AuthLoginExceptionType
    {
        UserNotFound,
        EmailNotFound,
        CredentialsMismatch
    }
    public class AuthLoginException(AuthLoginExceptionType exceptionType, string message) : Exception(message)
    {
        public AuthLoginExceptionType ExceptionType { get; private set; } = exceptionType;
    }
}
