namespace Application.Common.Validators.Auth;

public class LoginUserDTOValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserDTOValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(30);
        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6)
            .MaximumLength(30);
    }
}