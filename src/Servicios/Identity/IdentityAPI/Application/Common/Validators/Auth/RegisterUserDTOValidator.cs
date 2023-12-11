namespace Application.Common.Validators.Auth;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserDTOValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(30);
        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6)
            .MaximumLength(30)
            .Matches(x => x.Password);
        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(30)
            .Equal(x => x.Password).WithMessage(
                "El campo 'Confirmación de Contraseña' no concide con el campo 'Contraseña'.");
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}