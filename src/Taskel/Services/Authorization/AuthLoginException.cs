namespace Taskel.Services.Authorization
{
    public enum AuthLoginExceptionType
    {
        NotFound,
        DatabaseError,
        CredentialsMismatch
    }
    public class AuthLoginException(AuthLoginExceptionType exceptionType, string message) : Exception(message)
    {
        public AuthLoginExceptionType ExceptionType { get; private set; } = exceptionType;
    }
}
