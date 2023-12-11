namespace Application.Common.DTOs.FavoriteItem;

public class UpdateFavoriteItemRequest
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La descripción es requerida.")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es requerido.")]
    public decimal Precio { get; set; }
}