namespace Domain.Exceptions;

public sealed class ItemNotFoundException : Exception
{
    public ItemNotFoundException(Guid itemId) :
     base($"El item con identificador {itemId} no se encontró.")
    { }
}