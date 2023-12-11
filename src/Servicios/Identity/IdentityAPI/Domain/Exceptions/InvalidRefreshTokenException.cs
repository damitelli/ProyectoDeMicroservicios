namespace Domain.Exceptions;

public sealed class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException() : base("El token no es valido.")
    { }
}