namespace IntegrationTests.API;

public class AuthIntegrationTests : BaseClassFixture
{
    public AuthIntegrationTests(CustomWebApplicationFactory<Program> customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Register_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/register";
        var request = new RegisterUserRequest
        {
            FirstName = "a",
            LastName = "a",
            UserName = "a",
            Password = "1",
            ConfirmPassword = "12",
            Email = "a"
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_SuccessWhenPassValidRequest()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/register";
        var request = new RegisterUserRequest
        {
            FirstName = "abc",
            LastName = "abc",
            UserName = "auth-test-usuario",
            Password = "123456",
            ConfirmPassword = "123456",
            Email = "auth-test-usuario@email"
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("Usuario registrado con éxito.", responseString);
    }



    [Fact]
    public async Task Login_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/login";
        var request = new LoginUserRequest
        {
            UserName = "abc",
            Password = "123456",
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("incorrectos.", responseString);
    }

    [Fact]
    public async Task Login_SuccessWhenPassValidRequest()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/login";
        var request = new LoginUserRequest
        {
            UserName = "auth-test-usuario",
            Password = "123456",
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Inicio de sesión exitoso.", responseString);
    }



    [Fact]
    public async Task Generate_New_Token_ErrorWhenPassInvalidCookieData()
    {
        // Arrenge
        const string refreshUri = "v1/userauthentication/new-token";

        // Act
        var refreshResponse = await Client.PostAsJsonAsync(refreshUri, "");
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(refreshResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, refreshResponse.StatusCode);
        Assert.Contains("El token no es valido.", responseString);
    }

    [Fact]
    public async Task Generate_New_Token_SuccessWhenPassValidCookieData()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        const string refreshUri = "v1/userauthentication/new-token";

        var loginRequest = new LoginUserRequest
        {
            UserName = "usuariodeprueba",
            Password = "User!123456",
        };

        // Act
        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var refreshResponse = await Client.PostAsJsonAsync(refreshUri, "");
        refreshResponse.EnsureSuccessStatusCode();
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        Assert.Contains("Nuevo token generado con éxito.", responseString);
    }



    [Fact]
    public async Task Revoke_Refresh_Token_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { };

        // Act
        var refreshResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(refreshResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, refreshResponse.StatusCode);
        Assert.Contains("Token requerido.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_ErrorWhenPassNonexistentToken()
    {
        // Arrenge
        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { Token = "tokenInexitente" };

        // Act
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(revokeResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, revokeResponse.StatusCode);
        Assert.Contains("El token no es valido.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_SuccessWhenPassValidCookieData()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";

        var loginRequest = new LoginUserRequest
        {
            UserName = "moderator",
            Password = "M!123456",
        };

        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { };

        // Act
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        revokeResponse.EnsureSuccessStatusCode();
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, revokeResponse.StatusCode);
        Assert.Contains("Token revocado con éxito.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_SuccessWhenPassValidToken()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        var loginRequest = new LoginUserRequest
        {
            UserName = "usuariodeprueba",
            Password = "User!123456",
        };

        //Inicia sesión para generar un nuevo refresh token. 
        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var getAllTokensByUserIdUri = $"v1/users/{userId}/refresh-tokens-by-user-id/";

        var response = await Client.GetAsync(getAllTokensByUserIdUri);

        var getAllTokensResponseJson = await response.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<RefreshTokenDTO>>>();

        //Asigna a la variable "token" el valor del refresh token 
        //generado en el inicio de sesión previo.
        var token = getAllTokensResponseJson?.Data
            .Select(x => x.Token)
            .LastOrDefault();

        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest
        {
            Token = $"{token}"
        };

        // Act
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        revokeResponse.EnsureSuccessStatusCode();
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, revokeResponse.StatusCode);
        Assert.Contains("Token revocado con éxito.", responseString);
    }
}