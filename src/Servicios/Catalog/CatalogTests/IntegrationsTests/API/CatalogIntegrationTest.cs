namespace IntegrationTests.API;

public class CatalogIntegrationTests : BaseClassFixture
{
    public CatalogIntegrationTests(
        CustomWebApplicationFactory<Program> customWebApplicationFactory) :
         base(customWebApplicationFactory)
    { }

    [Fact]
    public async Task Get_All_Items_SuccessWhenIsInvoked()
    {
        // Arrange
        const string requestUri = "v1/items/all";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }



    [Fact]
    public async Task Create_Item_ErrorWhenUserIsAnonymous()
    {
        // Arrange
        const string requestUri = "v1/items/create";

        var request = new CreateItemRequest
        {
            Nombre = "TestingItem",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_Item_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrange
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string requestUri = "v1/items/create";

        var request = new CreateItemRequest
        {
            Nombre = "TestingItem",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Create_Item_ErrorWhenPassInvalidRequest()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string requestUri = "v1/items/create";

        var request = "Invalid Create Item Request";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Create_Item_SuccessWhenPassJwtTokenClaimsandValidRequest()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string requestUri = "v1/items/create";

        var request = new CreateItemRequest
        {
            Nombre = "TestingItem",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("Item creado con éxito.", responseString);
    }



    [Fact]
    public async Task Get_Item_By_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrange
        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/items/{itemId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_Item_By_Id_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrange
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
           x => x.Nombre == "TestingItem")
           .Select(x => x.Id)
           .FirstOrDefault();

        var requestUri = $"v1/items/{itemId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_Item_By_Id_ErrorWhenPassNonexistentItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var itemId = "itemIdInexistente";

        var requestUri = $"v1/items/{itemId}";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_Item_By_Id_SuccessWhenPassValidJwtTokenAndValidItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string createItemUri = "v1/items/create";

        var createItemRequest = new CreateItemRequest
        {
            Nombre = "ItemToGetById",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        //Se crea un nuevo Item para utilizar en el presente test.
        var createResponse = await Client.PostAsJsonAsync(createItemUri, createItemRequest);
        createResponse.EnsureSuccessStatusCode();

        var createResponseJson = await createResponse.Content.
        ReadFromJsonAsync<ApiResponse<ItemDTO>>();

        var itemIdString = createResponseJson?.Data.Id.ToString();

        var requestUri = $"v1/items/{itemIdString}";

        // Act
        var response = await Client.GetAsync(requestUri);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(itemIdString, responseString);
    }



    [Fact]
    public async Task Update_Item_ErrorWhenUserIsAnonymous()
    {
        // Arrange
        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/update/{itemId}";

        var updateItemRequest = new UpdateItemRequest
        {
            Nombre = "TestingItemUpdated",
            Descripcion = "Descripcion del Item Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Update_Item_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrange
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/update/{itemId}";

        var updateItemRequest = new UpdateItemRequest
        {
            Nombre = "TestingItemUpdated",
            Descripcion = "Descripcion del Item Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Update_Item_ErrorWhenPassInvalidRequest()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/update/{itemId}";

        var updateItemRequest = "Invalid Update Request";

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Update_Item_ErrorWhenPassNonexistentItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var itemId = "ItemIdInexitente";
        var requestUri = $"v1/items/update/{itemId}";

        var updateItemRequest = new UpdateItemRequest
        {
            Nombre = "TestingItemUpdated",
            Descripcion = "Descripcion del Item Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateItemRequest);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_Item_SuccessWhenPassJwtTokenClaimsAndValidRequest()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string createItemUri = "v1/items/create";

        var createItemRequest = new CreateItemRequest
        {
            Nombre = "ItemToUpdate",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        //Se crea un nuevo Item para utilizar en el presente test.
        var createResponse = await Client.PostAsJsonAsync(createItemUri, createItemRequest);
        createResponse.EnsureSuccessStatusCode();

        var createResponseJson = await createResponse.Content.
        ReadFromJsonAsync<ApiResponse<ItemDTO>>();

        var itemId = createResponseJson?.Data.Id;

        var requestUri = $"v1/items/update/{itemId}";

        var updateItemRequest = new UpdateItemRequest
        {
            Nombre = "ItemSuccessfullyUpdated",
            Descripcion = "Descripcion del Item Updated",
            Precio = 2
        };

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, updateItemRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item actualizado con éxito.", responseString);
    }



    [Fact]
    public async Task Delete_Item_ErrorWhenUserIsAnonymous()
    {
        // Arrange
        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/delete/{itemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Item_ErrorWhenPassInvalidJwtTokenClaims()
    {
        // Arrange
        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/delete/{itemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Item_ErrorWhenPassNonexistentItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var itemId = "ItemIdInexitente";
        var requestUri = $"v1/items/delete/{itemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Item_SuccessWhenPassValidJwtTokenClaimsAndItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string createItemUri = "v1/items/create";

        var createItemRequest = new CreateItemRequest
        {
            Nombre = "ItemToDelete",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        //Se crea un nuevo Item para utilizar en el presente test.
        var createResponse = await Client.PostAsJsonAsync(createItemUri, createItemRequest);
        createResponse.EnsureSuccessStatusCode();

        var createResponseJson = await createResponse.Content.
        ReadFromJsonAsync<ApiResponse<ItemDTO>>();

        var itemId = createResponseJson?.Data.Id;

        var requestUri = $"v1/items/delete/{itemId}";

        // Act
        var response = await Client.DeleteAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Item eliminado con éxito.", responseString);
    }



    [Fact]
    public async Task Add_Item_To_List_ErrorWhenUserIsAnonymous()
    {
        // Arrange
        const string getAllItemsUri = "v1/items/all";
        var allItemsResponse = await Client.GetAsync(getAllItemsUri);
        allItemsResponse.EnsureSuccessStatusCode();

        var getAllResponseJson = await allItemsResponse.Content.ReadFromJsonAsync<ApiResponse<IReadOnlyList<ItemDTO>>>();

        var itemId = getAllResponseJson?.Data.Where(
            x => x.Nombre == "TestingItem")
            .Select(x => x.Id)
            .FirstOrDefault();

        var requestUri = $"v1/items/additemtolist/{itemId}";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Add_Item_To_List_ErrorWhenPassNonexistentItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        var itemId = "ItemIdInexitente";
        var requestUri = $"v1/items/additemtolist/{itemId}";

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Add_Item_To_List_SuccessWhenPassValidJwtTokenClaimsAndItemId()
    {
        // Arrange
        var superAdminAccessToken = JwtTokenProvider.GenerateSuperAdminAccessToken();

        //Se asigna el accessToken de super admin al valor de la cabecera de autenticación para,
        //más abajo, poder crear un nuevo Item.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", superAdminAccessToken);

        const string createItemUri = "v1/items/create";

        var createItemRequest = new CreateItemRequest
        {
            Nombre = "ItemToAdd",
            Descripcion = "Descripcion del Item",
            Precio = 1
        };

        //Se crea un nuevo Item para utilizar en el presente test.
        var createResponse = await Client.PostAsJsonAsync(createItemUri, createItemRequest);
        createResponse.EnsureSuccessStatusCode();

        var createResponseJson = await createResponse.Content.
        ReadFromJsonAsync<ApiResponse<ItemDTO>>();

        var itemId = createResponseJson?.Data.Id;

        var requestUri = $"v1/items/additemtolist/{itemId}";

        var moderatorAccessToken = JwtTokenProvider.GenerateModeratorAccessToken();

        //Se asigna el accessToken de moderator al valor de la cabecera de autenticación para
        //comprobar que no son necesarias las credenciales de SuperAdmin.
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", moderatorAccessToken);

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, "");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("El item ha sido enviado a tu lista de favoritos exitosamente.", responseString);
    }
}
