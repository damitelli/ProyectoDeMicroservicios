namespace UnitTests.Items;

public class CreateItemDTOValidatorTest
{
    private readonly CreateItemDTOValidator _createValidator;

    public CreateItemDTOValidatorTest() => _createValidator = new CreateItemDTOValidator();

    // Arrange
    public class CreateItemRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new CreateItemRequest {
                Nombre = null, Descripcion = null, Precio = 0 }};
            yield return new object[] { new CreateItemRequest {
                Nombre = "", Descripcion = "", Precio = 0
                }};
            yield return new object[] { new CreateItemRequest {
                 Nombre = "a", Descripcion = "a", Precio = -1
                 }};
            yield return new object[] { new CreateItemRequest {
                    Nombre =
                    "EstaEsUnaCadenaCon31Caracteres!",
                    Descripcion = "EstaEsUnaCadenaCon31Caracteres!EstaEsUnaCadenaCon62Caracteres!EstaEsUnaCadenaCon93Caracteres!EstaEsUnaCadenaCon125Caracteres!",
                    Precio = -1 }
                };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Theory]
    [ClassData(typeof(CreateItemRequestTestData))]
    public void Should_have_error(CreateItemRequest request)
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
        var model = new CreateItemRequest
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