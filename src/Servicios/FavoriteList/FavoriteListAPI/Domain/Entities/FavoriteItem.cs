namespace Domain.Entities;

public class FavoriteItem
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }
}