namespace Application.Common.DTOs.Auth;

public class RegisterUserRequest
{
    [Required(ErrorMessage = "El Nombre es requerido.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "El Apellido es requerido.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "El Usuario es requerido.")]
    public string UserName { get; init; }

    [Required(ErrorMessage = "La Contraseña es requerida.")]
    public string Password { get; init; }

    [Required(ErrorMessage = "La confirmación de la Contraseña es requerida.")]
    public string ConfirmPassword { get; init; }

    [Required(ErrorMessage = "El Email es requerido.")]
    public string Email { get; init; }
}