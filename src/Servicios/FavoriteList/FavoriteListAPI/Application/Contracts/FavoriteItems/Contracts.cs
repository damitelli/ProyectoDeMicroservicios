namespace Application.Contracts.Favoriteitems;

public record FavoriteItemsCreated(Guid UserId);

public record FavoriteItemsDeleted(Guid UserId);

public record ItemAddedToFavoriteItems(
    Guid UserId, Guid ItemId, string Nombre, string Descripcion, decimal Precio);

public record ItemRemovedFromFavoriteItems(Guid UserId, Guid ItemId);

