namespace IntegrationTests.Common;

public class BaseClassFixture : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public BaseClassFixture(CustomWebApplicationFactory<Program> factory) =>
        Client = factory.CreateClient();
}