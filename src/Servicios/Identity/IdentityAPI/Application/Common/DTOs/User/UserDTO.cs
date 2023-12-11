namespace Application.Common.DTOs.User;

public class UserDTO
{
    public string? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; init; }
    public string Email { get; init; }
}