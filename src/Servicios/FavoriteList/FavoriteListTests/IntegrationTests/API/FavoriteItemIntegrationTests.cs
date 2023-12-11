namespace IntegrationTests.API;

public class FavoriteItemIntegrationTests : BaseClassFixture
{
    public FavoriteItemIntegrationTests(
        CustomWebApplicationFactory<Program> customWebApplicationFactory) :
        base(customWebApplicationFactory)
    { }

    [Fact]
    public async Task Get_All_FavoriteItems_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        const string requestUri = "v1/favorite-items/all";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_FavoriteItems_SuccessWhenPassValidJwtToken()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string requestUri = "v1/favorite-items/all";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }




    [Fact]
    public async Task Create_FavoriteItem_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        const string requestUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "TestingFavoriteItem",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, createFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_FavoriteItem_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string requestUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "TestingFavoriteItem",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, createFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Create_FavoriteItem_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string requestUri = "v1/favorite-items/create";

        var favoriteItemRequest = "Invalid Create Item Request";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, favoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    }

    [Fact]
    public async Task Create_FavoriteItem_SuccessWhenPassValidJwtTokenClaimsAndValidRequest()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var requestUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "TestingFavoriteItem",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, createFavoriteItemRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("Item favorito creado con éxito", responseString);
    }



    [Fact]
    public async Task Get_FavoriteItem_By_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para
        //poder acceder a todos los Favorite Items.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var getAllFavoriteItemsUri = "v1/favorite-items/all";
        var allFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        allFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allFavoriteItemsResponse.Content.
        ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/{favoriteItemId}";

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
    public async Task Get_FavoriteItem_By_Id_ErrorWhenPassNonexistentFavoriteItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var favoriteItemId = "favoriteItemIdInexistente";

        var requestUri = $"v1/favorite-items/{favoriteItemId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Get_FavoriteItem_By_Id_SuccessWhenPassValidJwtTokenAndValidItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "FavoriteItemToGetById",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemIdString = createFITResponseJson?.Data.Id.ToString();

        var requestUri = $"v1/favorite-items/{favoriteItemIdString}";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(favoriteItemIdString, responseString);

    }



    [Fact]
    public async Task Update_FavoriteItem_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todos los Favorite Items.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string getAllFavoriteItemsUri = "v1/favorite-items/all";
        var allFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        allFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allFavoriteItemsResponse.Content.
        ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/update/{favoriteItemId}";

        var updateFavoriteItemRequest = new UpdateFavoriteItemRequest
        {
            Nombre = "FavoriteItemSuccessfullyUpdated",
            Descripcion = "Descripcion del favoriteItem Updated",
            Precio = 2
        };

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [Fact]
    public async Task Update_FavoriteItem_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string getAllFavoriteItemsUri = "v1/favorite-items/all";
        var allFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        allFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allFavoriteItemsResponse.Content.
        ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/update/{favoriteItemId}";

        var updateFavoriteItemRequest = new UpdateFavoriteItemRequest
        {
            Nombre = "FavoriteItemSuccessfullyUpdated",
            Descripcion = "Descripcion del favoriteItem Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Update_FavoriteItem_ErrorWhenPassInvalidRequest()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string getAllFavoriteItemsUri = "v1/favorite-items/all";
        var allFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        allFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allFavoriteItemsResponse.Content.
        ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/update/{favoriteItemId}";

        var updateFavoriteItemRequest = "Invalid Update Request";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    }
    [Fact]
    public async Task Update_FavoriteItem_ErrorWhenPassNonexistentFavoriteItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var favoriteItemId = "ItemIdInexitente";

        var requestUri = $"v1/favorite-items/update/{favoriteItemId}";

        var updateFavoriteItemRequest = new UpdateFavoriteItemRequest
        {
            Nombre = "FavItemSuccessfullyUpdated",
            Descripcion = "Descripcion del favoriteItem Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateFavoriteItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_FavoriteItem_SuccessWhenPassValidJwtTokenClaimsAndValidData()
    {

        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "FavoriteItemToUpdate",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        // Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var requestUri = $"v1/favorite-items/update/{favoriteItemId}";

        var updateFavoriteItemRequest = new UpdateFavoriteItemRequest
        {
            Nombre = "FavItemSuccessfullyUpdated",
            Descripcion = "Descripcion del favoriteItem Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateFavoriteItemRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item favorito actualizado con éxito.", responseString);

    }



    [Fact]
    public async Task Delete_FavoriteItem_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder acceder a todos los Favorite Items.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string getAllFavoriteItemsUri = "v1/favorite-items/all";
        var getAllFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        getAllFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await getAllFavoriteItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/delete/{favoriteItemId}";

        //Se asigna como null al valor de la cabecera de autenticación para
        //poder, efectivamente, hacer una petición anónima.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", null);

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [Fact]
    public async Task Delete_FavoriteItem_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrenge
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string getAllFavoriteItemsUri = "v1/favorite-items/all";
        var getAllFavoriteItemsResponse = await Client.GetAsync(getAllFavoriteItemsUri);
        getAllFavoriteItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await getAllFavoriteItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<FavoriteItemDTO>>>();

        var favoriteItemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingFavoriteItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/favorite-items/delete/{favoriteItemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

    }

    [Fact]
    public async Task Delete_FavoriteItems_ErrorWhenPassNonexistentFavoriteItemId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var favoriteItemId = "favoriteItemIdInexistente";

        var requestUri = $"v1/favorite-items/delete/{favoriteItemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Delete_FavoriteItem_SuccessWhenPassValidJwtTokenClaimsAndValidFavoriteItemsId()
    {
        // Arrenge
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var createFavoriteItemUri = "v1/favorite-items/create";

        var createFavoriteItemRequest = new CreateFavoriteItemRequest
        {
            Nombre = "FavoriteItemToDelete",
            Descripcion = "Descripcion del favoriteItem",
            Precio = 1
        };

        //Se crea un nuevo Favorite Item para utilizar en el presente test.
        var createFavoriteItemResponse = await Client.PostAsJsonAsync(
            createFavoriteItemUri, createFavoriteItemRequest);
        createFavoriteItemResponse.EnsureSuccessStatusCode();

        var createFITResponseJson = await createFavoriteItemResponse.Content.ReadFromJsonAsync<ApiResponse<FavoriteItemDTO>>();

        var favoriteItemId = createFITResponseJson?.Data.Id;

        var requestUri = $"v1/favorite-items/delete/{favoriteItemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item favorito eliminado con éxito.", responseString);
    }
}