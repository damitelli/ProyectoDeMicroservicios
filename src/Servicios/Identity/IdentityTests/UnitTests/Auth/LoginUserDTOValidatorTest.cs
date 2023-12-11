namespace UnitTests.Auth;

public class LoginUserDTOValidatorTest
{
    private readonly LoginUserDTOValidator _loginValidator;

    public LoginUserDTOValidatorTest() => _loginValidator = new LoginUserDTOValidator();

    // Arrange
    public class LoginUserRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new LoginUserRequest { UserName = null, Password = null } };
            yield return new object[] { new LoginUserRequest { UserName = "", Password = "" } };
            yield return new object[] { new LoginUserRequest { UserName = "a", Password = "1" } };
            yield return new object[] { new LoginUserRequest { UserName = "ab", Password = "12" } };
            yield return new object[] { new LoginUserRequest { UserName = "", Password = "123" } };
            yield return new object[] { new LoginUserRequest { UserName = "", Password = "1234" } };
            yield return new object[] { new LoginUserRequest { UserName = "", Password = "12345" } };
            yield return new object[] { new LoginUserRequest {
                UserName =
                "EstaEsUnaCadenaCon31Caracteres!",
                Password = "EstaEsUnaCadenaCon31Caracteres!EstaEsUnaCadenaCon62Caracteres!EstaEsUnaCadenaCon93Caracteres!EstaEsUnaCadenaCon125Caracteres!" } };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    [Theory]
    [ClassData(typeof(LoginUserRequestTestData))]
    public void Should_have_error(LoginUserRequest request)
    {
        // Act
        var result = _loginValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_be_successful()
    {
        // Arrange
        var model = new LoginUserRequest { UserName = "abc", Password = "123456" };

        // Act
        var result = _loginValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}