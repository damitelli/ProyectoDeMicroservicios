namespace Domain.Exceptions;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId) :
     base($"El usuario con identificador {userId} no se encontró.")
    { }
}
