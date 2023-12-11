namespace Application.Common.DTOs.User;

public class UpdateUserInfoRequest
{
    [Required(ErrorMessage = "El Nombre es requerido.")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "El Apellido es requerido.")]
    public string LastName { get; init; }

    [Required(ErrorMessage = "El Usuario es requerido.")]
    public string UserName { get; init; }

    [Required(ErrorMessage = "El Email es requerido.")]
    public string Email { get; set; }
}