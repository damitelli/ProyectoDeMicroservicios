namespace IntegrationTests.API;

public class UserFavoriteListntegrationTests : BaseClassFixture
{
    public UserFavoriteListntegrationTests(
        CustomWebApplicationFactory<Program> customWebApplicationFactory) :
        base(customWebApplicationFactory)
    { }

    [Fact]
    public async Task Get_All_UserFavoriteList_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        const string requestUri = "v1/user-favorite-list/all";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_UserFavoriteList_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string requestUri = "v1/user-favorite-list/all";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_UserFavoriteList_SuccessWhenPassValidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string requestUri = "v1/user-favorite-list/all";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }



    [Fact]
    public async Task Create_UserFavoriteList_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var userId = Guid.NewGuid();
        var requestUri = $"v1/user-favorite-list/create/{userId}";

        var userFavoriteListRequest = new UserFavoriteList
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, userFavoriteListRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_UserFavoriteList_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        var userId = Guid.NewGuid();
        var requestUri = $"v1/user-favorite-list/create/{userId}";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Create_UserFavoriteList_SuccessWhenPassValidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var requestUri = $"v1/user-favorite-list/create/{userId}";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, "");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("Lista de favoritos creada con éxito.", responseString);
    }



    [Fact]
    public async Task Get_UserFavoriteList_By_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}";

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
    public async Task Get_UserFavoriteList_By_Id_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}";

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
    public async Task Get_UserFavoriteList_By_Id_ErrorWhenPassNonexistentId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userFavoriteListId = "listIdInexistente";

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_UserFavoriteList_By_Id_SuccessWhenPassValidJwtTokenClaimsAndId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";

        var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
            createUserFavoriteListUri, "");
        createUserFavoriteListResponse.EnsureSuccessStatusCode();

        var createUFLResponseJson = await createUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListId = createUFLResponseJson?.Data.Id.ToString();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(userFavoriteListId, responseString);
    }



    [Fact]
    public async Task Get_UserFavoriteList_By_UserId_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListUserId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/user/{userFavoriteListUserId}";

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
    public async Task Get_UserFavoriteList_By_UserId_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListUserId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/user/{userFavoriteListUserId}";

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
    public async Task Get_UserFavoriteList_By_UserId_ErrorWhenPassNonexistentUserId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userFavoriteListUserId = "userIdInexitente";

        var requestUri = $"v1/user-favorite-list/user/{userFavoriteListUserId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_UserFavoriteList_By_UserId_SuccessWhenPassValidJwtTokenAndUserId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";

        var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
            createUserFavoriteListUri, "");
        createUserFavoriteListResponse.EnsureSuccessStatusCode();

        var createUFLResponseJson = await createUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListUserId = createUFLResponseJson?.Data.UserId.ToString();

        var requestUri = $"v1/user-favorite-list/user/{userFavoriteListUserId}";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(userFavoriteListUserId, responseString);
    }



    [Fact]
    public async Task Add_FavoriteItem_To_UserFavoriteList_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Add_FavoriteItem_To_UserFavoriteList_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";

        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //hacer una petición sin las credenciales pertinentes.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Add_FavoriteItem_To_UserFavoriteList_ErrorWhenPassNonexistentListId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userFavoriteListId = "listIdInexistente";

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Add_FavoriteItem_To_UserFavoriteList_ErrorWhenPassNonExistentFavoriteItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var favoriteItemId = "favoriteItemIdInexistente";

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Add_FavoriteItem_To_UserFavoriteList_SuccessWhenPassValidJwtTokenListIdAndItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";

        var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
            createUserFavoriteListUri, "");
        createUserFavoriteListResponse.EnsureSuccessStatusCode();

        var createUFLResponseJson = await createUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListId = createUFLResponseJson?.Data.Id;

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "FavoriteItemToAdd",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var requestUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(
            "Se agregó el Item favorito a la lista de favoritos con éxito.", responseString);
    }



    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListUserId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todas las Favorite Lists.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListUserId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //hacer una petición sin las credenciales pertinentes.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_ErrorWhenPassNonexistentUserId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userFavoriteListUserId = "userIdInexistente";

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllFITResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllFITResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_ErrorWhenPassNonexistentFavoriteItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllUserFavoriteListUri = "v1/user-favorite-list/all";
        var getAllUserFavoriteListResponse = await Client.GetAsync(getAllUserFavoriteListUri);
        getAllUserFavoriteListResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<UserFavoriteListDTO>>>();

        var userFavoriteListUserId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var favoriteItemId = "favoriteItemInexistente";

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_ErrorWhenPassFavoriteItemNotWhitingList()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";

        var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
            createUserFavoriteListUri, "");
        createUserFavoriteListResponse.EnsureSuccessStatusCode();

        var createUFLResponseJson = await createUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListUserId = createUFLResponseJson?.Data.UserId;

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "ItemCreadoParaTesting",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_UserFavoriteList_SuccessWhenPassValidJwtTokenAndCorrespondingData()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = Guid.NewGuid();
        var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";

        var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
            createUserFavoriteListUri, "");
        createUserFavoriteListResponse.EnsureSuccessStatusCode();

        var createUFLResponseJson = await createUserFavoriteListResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListId = createUFLResponseJson?.Data.Id;

        var userFavoriteListUserId = createUFLResponseJson?.Data.UserId;

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "ItemCreadoParaTesting",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var addFITtoUFLUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";
        var addFITtoUFLResponse = await Client.PutAsJsonAsync(addFITtoUFLUri, "");
        addFITtoUFLResponse.EnsureSuccessStatusCode();

        var requestUri = $"v1/user-favorite-list/{userFavoriteListUserId}/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item favorito eliminado de la lista de favoritos con éxito.", responseString);
    }



    [Fact]
    public async Task Remove_FavoriteItem_From_Current_UserFavoriteList_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todos las Favorite Items.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllFavoriteItemUri = "v1/favorite-items/all";
        var getAllFavoriteItemResponse = await Client.GetAsync(getAllFavoriteItemUri);
        getAllFavoriteItemResponse.EnsureSuccessStatusCode();

        var getAllUFLResponseJson = await getAllFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllUFLResponseJson?.Data.
        Select(x => x.Id).FirstOrDefault();

        var requestUri = $"v1/user-favorite-list/removeitem/{favoriteItemId}";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_Current_UserFavoriteList_ErrorWhenPassNonexistentItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = "b6d099b2-2878-4bab-b40e-cbbdc73e2cd6";
        var getUserFavoriteListByUserIdUri = $"v1/user-favorite-list/user/{userId}";

        var getUserFavoriteListByUserIdResponse = await Client.GetAsync(getUserFavoriteListByUserIdUri);

        if (getUserFavoriteListByUserIdResponse.StatusCode == HttpStatusCode.NotFound)
        {
            var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";
            var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
                createUserFavoriteListUri, "");
            createUserFavoriteListResponse.EnsureSuccessStatusCode();
        }

        var favoriteItemId = "favoriteItemIdInexistente";

        var requestUri = $"v1/user-favorite-list/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_Current_UserFavoriteList_ErrorWhenPassFavoriteItemNotWhitingList()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = "b6d099b2-2878-4bab-b40e-cbbdc73e2cd6";
        var getUserFavoriteListByUserIdUri = $"v1/user-favorite-list/user/{userId}";

        var getUserFavoriteListByUserIdResponse = await Client.GetAsync(getUserFavoriteListByUserIdUri);

        if (getUserFavoriteListByUserIdResponse.StatusCode == HttpStatusCode.NotFound)
        {
            var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";
            var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
                createUserFavoriteListUri, "");
            createUserFavoriteListResponse.EnsureSuccessStatusCode();
        }

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "ItemCreadoParaTesting",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var requestUri = $"v1/user-favorite-list/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_FavoriteItem_From_Current_UserFavoriteList_SuccessWhenPassValidJwtTokenAndData()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var userId = "b6d099b2-2878-4bab-b40e-cbbdc73e2cd6";
        var getUserFavoriteListByUserIdUri = $"v1/user-favorite-list/user/{userId}";

        var getUserFavoriteListByUserIdResponse = await Client.GetAsync(getUserFavoriteListByUserIdUri);

        if (getUserFavoriteListByUserIdResponse.StatusCode == HttpStatusCode.NotFound)
        {
            var createUserFavoriteListUri = $"v1/user-favorite-list/create/{userId}";
            var createUserFavoriteListResponse = await Client.PostAsJsonAsync(
                createUserFavoriteListUri, "");
            createUserFavoriteListResponse.EnsureSuccessStatusCode();
        }

        var getUFLResponseJson = await getUserFavoriteListByUserIdResponse.Content.ReadFromJsonAsync<ApiResponse<UserFavoriteListDTO>>();

        var userFavoriteListId = getUFLResponseJson?.Data.Id;

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "ItemCreadoParaTesting",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var addFITtoUFLUri = $"v1/user-favorite-list/{userFavoriteListId}/additem/{favoriteItemId}";
        var addFITtoUFLResponse = await Client.PutAsJsonAsync(addFITtoUFLUri, "");
        addFITtoUFLResponse.EnsureSuccessStatusCode();

        var requestUri = $"v1/user-favorite-list/removeitem/{favoriteItemId}";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item favorito eliminado de la lista de favoritos con éxito.", responseString);
    }
}