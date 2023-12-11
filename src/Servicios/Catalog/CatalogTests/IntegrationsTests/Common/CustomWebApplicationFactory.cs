namespace IntegrationTests.Common;

public class CustomWebApplicationFactory<TEntryPoint> :
WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = JwtTokenProvider.Issuer,
                            ValidAudience = JwtTokenProvider.Audience,
                            IssuerSigningKey = JwtTokenProvider.SecurityKey,
                            ClockSkew = TimeSpan.Zero
                        };
                    }
            );
        });

        return base.CreateHost(builder);
    }
}