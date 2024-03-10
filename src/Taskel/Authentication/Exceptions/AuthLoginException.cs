namespace Taskel.Authentication.Exceptions
{
    public enum AuthLoginExceptionType
    {
        UserNotFound,
        InvalidEmail,
        EmailNotFound,
        CredentialsMismatch
    }
    public class AuthLoginException(AuthLoginExceptionType exceptionType, string message) : Exception(message)
    {
        public AuthLoginExceptionType ExceptionType { get; private set; } = exceptionType;
    }
}
