namespace UnitTests.Favoriteitems;

public class CreateFavoriteItemDTOValidatorTest
{
    private readonly CreateFavoriteItemDTOValidator _createValidator;

    public CreateFavoriteItemDTOValidatorTest() =>
    _createValidator = new CreateFavoriteItemDTOValidator();

    // Arrange
    public class CreateFavoriteItemRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new CreateFavoriteItemRequest{
                Nombre = null, Descripcion = null, Precio = 0}};

            yield return new object[] {new CreateFavoriteItemRequest{
                Nombre = "", Descripcion = "", Precio = 0}};

            yield return new object[] {new CreateFavoriteItemRequest{
                Nombre = "A", Descripcion = "B", Precio = -1}};

            yield return new object[] {new CreateFavoriteItemRequest{
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
    [ClassData(typeof(CreateFavoriteItemRequestTestData))]
    public void Should_have_error(CreateFavoriteItemRequest request)
    {
        // Act
        var result = _createValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nombre);
        result.ShouldHaveValidationErrorFor(x => x.Descripcion);
        result.ShouldHaveValidationErrorFor(x => x.Precio);
    }

    [Fact]
    public void Should_be_successful()
    {
        // Arrange
        var model = new CreateFavoriteItemRequest
        {
            Nombre = "Test-Item",
            Descripcion = "Test-Item",
            Precio = 1
        };

        // Act
        var result = _createValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nombre);
        result.ShouldNotHaveValidationErrorFor(x => x.Descripcion);
        result.ShouldNotHaveValidationErrorFor(x => x.Precio);
    }
}