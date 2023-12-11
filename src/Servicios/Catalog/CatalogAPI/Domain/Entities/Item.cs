namespace Domain.Entities;

public class Item
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public DateTimeOffset FechaDeCreacion { get; set; }
}