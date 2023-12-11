namespace Application.Common.DTOs.Auth;

public class LoginUserRequest
{
    [Required(ErrorMessage = "El Usuario es requerido.")]
    public string UserName { get; init; }

    [Required(ErrorMessage = "La Contraseña es requerida.")]
    public string Password { get; init; }
}
