namespace Application.Common.Validators.FavoriteItem;

public class UpdateFavoriteItemDTOValidator : AbstractValidator<UpdateFavoriteItemRequest>
{
    public UpdateFavoriteItemDTOValidator()
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
            .GreaterThan(0);
    }
}