namespace Application.Contracts.Items;

public record ItemModified(Guid ItemId, string Nombre, string Descripcion, decimal Precio);
public record ItemDeleted(Guid ItemId);
public record ItemAddedToFavoriteList(
    string UserId, Guid ItemId, string Nombre, string Descripcion, decimal Precio);