namespace Domain.Exceptions;

public sealed class ListByUserIdNotFoundException : Exception
{
    public ListByUserIdNotFoundException(Guid userId) :
     base($"No se encuentra ninguna lista para el usuario con identificador {userId}.")
    { }
}