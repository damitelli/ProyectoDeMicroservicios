namespace Application.Common.Validators.Items;

public class CreateItemDTOValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemDTOValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(x => x.Descripcion)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(100);
        RuleFor(x => x.Precio)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0);
    }
}
