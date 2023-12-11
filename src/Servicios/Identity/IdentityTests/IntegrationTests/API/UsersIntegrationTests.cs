namespace IntegrationTests.API;

public class UsersIntegrationTests : BaseClassFixture
{
    public UsersIntegrationTests(CustomWebApplicationFactory<Program> customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Get_All_Users_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Users_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Users_SuccessWhenPassValidJwtToken()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("superadmin", responseString);
    }




    [Fact]
    public async Task Get_User_By_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/users/{userId}";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
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

        var requestUri = $"v1/users/{userId}";

        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //hacer una petición sin las credenciales pertinentes.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_ErrorWhenPassInexistentUserId()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        var userId = "userIdInexitente";
        var requestUri = $"v1/users/{userId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_SuccessWhenPassValidJwtTokenAndValidUserId()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userIdString = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault()?.ToString();

        var requestUri = $"v1/users/{userIdString}";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(userIdString, responseString);
    }



    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/users/{userId}/refresh-tokens-by-user-id/";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
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

        var requestUri = $"v1/users/{userId}/refresh-tokens-by-user-id/";

        var moderatoraccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //hacer una petición sin las credenciales pertinentes.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatoraccessToken);

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenPassInexistentUserId()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        var userId = "userIdInexitente";
        var requestUri = $"v1/users/{userId}/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_SuccessWhenPassValidJwtTokenAndValidUserId()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        var loginRequest = new LoginUserRequest
        {
            UserName = "usuariodeprueba",
            Password = "User!123456",
        };

        //Inicia sesión para asegurar de que exista al menos un refresh token. 
        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();

        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/users/{userId}/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("token", responseString);
    }



    [Fact]
    public async Task Update_User_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/users/update/{userId}";

        var request = new UpdateUserInfoRequest
        {
            FirstName = "nuevo Nombre",
            LastName = "nuevo Apellido",
            UserName = "usuariodepruebaupdated",
            Email = "usuariodepruebaupdated@email"
        };

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder al id del usuariodeprueba.
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

        var requestUri = $"v1/users/update/{userId}";

        var request = new UpdateUserInfoRequest
        {
            FirstName = "Nuevo Nombre",
            LastName = "Nuevo Apellido",
            UserName = "usuariodepruebaupdated",
            Email = "usuariodepruebaupdated@email"
        };

        var moderatoraccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //hacer una petición sin las credenciales pertinentes.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatoraccessToken);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
            x => x.UserName == "usuariodeprueba")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/users/update/{userId}";

        var request = new UpdateUserInfoRequest
        {
            FirstName = "a",
            LastName = "a",
            UserName = "a",
            Email = "a"
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInexistentUserId()
    {
        // Arrenge
        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        var userId = "userIdInexitente";
        var request = new UpdateUserInfoRequest
        {
            FirstName = "nuevo Nombre",
            LastName = "nuevo Apellido",
            UserName = "usuariodepruebaupdated",
            Email = "usuariodepruebaupdated@email"
        };
        var requestUri = $"v1/users/update/{userId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_SuccessWhenPassValidUserIdAndValidRequest()
    {
        // Arrenge
        const string registerRequestUri = "v1/userauthentication/register";
        var registerRequest = new RegisterUserRequest
        {
            FirstName = "abc",
            LastName = "abc",
            UserName = "user-test-usuario",
            Password = "123456",
            ConfirmPassword = "123456",
            Email = "user-test-usuario@email"
        };

        //Se registra un usuario nuevo para utilizar en el presente test.
        await Client.PostAsJsonAsync(registerRequestUri, registerRequest);

        var accessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", accessToken);

        const string getAllUsersUri = "v1/users";

        var allUsersResponse = await Client.GetAsync(getAllUsersUri);
        allUsersResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allUsersResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserDTO>>>();

        var userId = getAllResponseJson?.Data.Where(
             x => x.UserName == "user-test-usuario")
             .Select(x => x.Id)
             .FirstOrDefault();

        var requestUri = $"v1/users/update/{userId}";

        var request = new UpdateUserInfoRequest
        {
            FirstName = "nuevo Nombre",
            LastName = "nuevo Apellido",
            UserName = "user-test-usuario-updated",
            Email = "user-test-usuario-updated@email"
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Usuario actualizado con éxito.", responseString);
    }
}