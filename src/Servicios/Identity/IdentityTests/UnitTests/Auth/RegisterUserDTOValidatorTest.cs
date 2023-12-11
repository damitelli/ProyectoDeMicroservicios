namespace UnitTests.Auth;

public class RegisterUserDTOValidatorTest
{
    private readonly RegisterUserDTOValidator _registerValidator;

    public RegisterUserDTOValidatorTest() => _registerValidator = new RegisterUserDTOValidator();

    // Arrange
    public class RegisterUserRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new RegisterUserRequest {
                FirstName = null, LastName = null, UserName = null,
                Password = null, ConfirmPassword = null, Email = null } };

            yield return new object[] { new RegisterUserRequest {
                FirstName = "", LastName = "", UserName = "",
                Password = "", ConfirmPassword = "", Email = "" } };

            yield return new object[] { new RegisterUserRequest {
                FirstName = "a", LastName = "a", UserName = "a",
                Password = "1", ConfirmPassword = "1", Email = "a" } };

            yield return new object[] { new RegisterUserRequest {
                 UserName = "ab", Password = "12", ConfirmPassword = "12" } };

            yield return new object[] { new RegisterUserRequest {
                Password = "123", ConfirmPassword = "123" } };

            yield return new object[] { new RegisterUserRequest {
                Password = "1234", ConfirmPassword = "1234" } };

            yield return new object[] { new RegisterUserRequest {
                Password = "12345", ConfirmPassword = "12345" } };

            yield return new object[] { new RegisterUserRequest {
                FirstName = "EstaEsUnaCadenaCon31Caracteres!",
                LastName = "EstaEsUnaCadenaCon31Caracteres!",
                UserName = "EstaEsUnaCadenaCon31Caracteres!",
                Password = "EstaEsUnaCadenaCon31Caracteres!",
                ConfirmPassword = "EstaEsUnaCadenaCon31Caracteres!" } };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Theory]
    [ClassData(typeof(RegisterUserRequestTestData))]
    public void Should_have_error(RegisterUserRequest request)
    {
        // Act
        var result = _registerValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Password);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_be_error_for_confirmPassword()
    {
        // Arrange
        var model = new RegisterUserRequest { Password = "123456", ConfirmPassword = "1234567" };

        // Act
        var result = _registerValidator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword)
        .WithErrorMessage("El campo 'Confirmación de Contraseña' no concide con el campo 'Contraseña'.");
    }

    [Fact]
    public void Should_be_successful()
    {
        // Arrange
        var model = new RegisterUserRequest
        {
            FirstName = "abc!",
            LastName = "abc",
            UserName = "abc",
            Password = "123456",
            ConfirmPassword = "123456",
            Email = "a@email"
        };

        // Act
        var result = _registerValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
        result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}