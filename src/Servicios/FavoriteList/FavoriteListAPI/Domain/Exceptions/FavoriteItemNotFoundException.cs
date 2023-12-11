namespace Domain.Exceptions;

public sealed class FavoriteItemNotFoundException : Exception
{
    public FavoriteItemNotFoundException(Guid favoriteItemId) :
     base($"El Item con identificador {favoriteItemId} no se encontró.")
    { }
}