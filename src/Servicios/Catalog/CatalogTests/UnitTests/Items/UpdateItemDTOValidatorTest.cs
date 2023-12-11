namespace UnitTests.Items;

public class UpdateItemDTOValidatorTest
{
    private readonly UpdateItemDTOValidator _updateValidator;

    public UpdateItemDTOValidatorTest() => _updateValidator = new UpdateItemDTOValidator();

    // Arrange
    public class UpdateItemRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new UpdateItemRequest {
                Nombre = null, Descripcion = null, Precio = 0}};
            yield return new object[] { new UpdateItemRequest {
                Nombre = "", Descripcion = "", Precio = 0}};
            yield return new object[] { new UpdateItemRequest {
                 Nombre = "a", Descripcion = "a", Precio = -1}};
            yield return new object[] { new UpdateItemRequest {
                    Nombre =
                    "EstaEsUnaCadenaCon31Caracteres!",
                    Descripcion = "EstaEsUnaCadenaCon31Caracteres!EstaEsUnaCadenaCon62Caracteres!EstaEsUnaCadenaCon93Caracteres!EstaEsUnaCadenaCon125Caracteres!",
                    Precio = -1 }};
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Theory]
    [ClassData(typeof(UpdateItemRequestTestData))]
    public void Should_have_error(UpdateItemRequest request)
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
        var model = new UpdateItemRequest
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