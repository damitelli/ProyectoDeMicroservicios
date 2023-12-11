namespace Domain.Exceptions;

public sealed class UserFavoriteListNotFoundException : Exception
{
    public UserFavoriteListNotFoundException(Guid userId) :
     base($"La lista de favoritos con identificador {userId} no se encontró")
    { }
}