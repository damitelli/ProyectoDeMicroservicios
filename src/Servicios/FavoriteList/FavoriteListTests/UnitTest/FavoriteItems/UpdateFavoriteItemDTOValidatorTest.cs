
namespace UnitTests.Favoriteitems;

public class UpdateFavoriteItemDTOValidatorTest
{
    private readonly UpdateFavoriteItemDTOValidator _updateValidator;

    public UpdateFavoriteItemDTOValidatorTest() =>
    _updateValidator = new UpdateFavoriteItemDTOValidator();

    // Arrange
    public class UpdateFavoriteItemRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new UpdateFavoriteItemRequest {
                Nombre = null, Descripcion = null, Precio = 0}};

            yield return new object[] {new UpdateFavoriteItemRequest {
                Nombre = "", Descripcion = "", Precio = 0}};

            yield return new object[] {new UpdateFavoriteItemRequest {
                Nombre = "A", Descripcion = "B", Precio = -1}};
            yield return new object[] {new UpdateFavoriteItemRequest{
                Nombre =
                "EstaEsUnaCadenaCon31Caracteres!",
                Descripcion = "EstaEsUnaCadenaCon31Caracteres!EstaEsUnaCadenaCon62Caracteres!EstaEsUnaCadenaCon93Caracteres!EstaEsUnaCadenaCon125Caracteres!",
                Precio = -1
            }};
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Theory]
    [ClassData(typeof(UpdateFavoriteItemRequestTestData))]
    public void Should_have_error(UpdateFavoriteItemRequest request)
    {
        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert 
        result.ShouldHaveValidationErrorFor(x => x.Nombre);
        result.ShouldHaveValidationErrorFor(x => x.Descripcion);
        result.ShouldHaveValidationErrorFor(x => x.Precio);
    }

    [Fact]
    public void Should_be_successful()
    {
        // Arrange 
        var model = new UpdateFavoriteItemRequest
        {
            Nombre = "Test-Item",
            Descripcion = "Test-Item",
            Precio = 1
        };

        // Act
        var result = _updateValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nombre);
        result.ShouldNotHaveValidationErrorFor(x => x.Descripcion);
        result.ShouldNotHaveValidationErrorFor(x => x.Precio);
    }
}