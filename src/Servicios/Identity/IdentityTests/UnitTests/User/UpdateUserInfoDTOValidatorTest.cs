namespace UnitTests.User;

public class UpdateUserInfoDTOValidatorTest
{
    private readonly UpdateUserInfoDTOValidator _updateValidator;

    public UpdateUserInfoDTOValidatorTest() => _updateValidator = new UpdateUserInfoDTOValidator();

    // Arrange
    public class UpdateUserInfoRequestTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new UpdateUserInfoRequest {
                FirstName = null, LastName = null, UserName = null, Email = null } };

            yield return new object[] { new UpdateUserInfoRequest {
                FirstName = "", LastName = "", UserName = "", Email = "" } };

            yield return new object[] { new UpdateUserInfoRequest {
                FirstName = "a", LastName = "a", UserName = "a", Email = "a" } };

            yield return new object[] { new UpdateUserInfoRequest { UserName = "ab" } };

            yield return new object[] { new UpdateUserInfoRequest {
                FirstName = "EstaEsUnaCadenaCon31Caracteres!",
                LastName = "EstaEsUnaCadenaCon31Caracteres!",
                UserName = "EstaEsUnaCadenaCon31Caracteres!"} };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    [Theory]
    [ClassData(typeof(UpdateUserInfoRequestTestData))]
    public void Should_have_error(UpdateUserInfoRequest request)
    {
        // Act
        var result = _updateValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_be_successful()
    {
        // Arrange
        var model = new UpdateUserInfoRequest
        {
            FirstName = "abc!",
            LastName = "abc",
            UserName = "abc",
            Email = "a@email"
        };

        // Act
        var result = _updateValidator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}