namespace Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Configura MongoDB
        BsonSerializerRegisterer.RegisterSerializer();

        services.Configure<FavoriteListDatabaseSettings>(
          configuration.GetSection(nameof(FavoriteListDatabaseSettings)));

        services.AddSingleton<IFavoriteListDatabaseSettings>(provider =>
        provider.GetRequiredService<IOptions<FavoriteListDatabaseSettings>>().Value);


        //Configura Servicios
        services.AddScoped<IFavoriteItemService, FavoriteItemService>();
        services.AddScoped<IUserFavoriteListService, UserFavoriteListService>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


        //Configura MassTransit y RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(AssemblyReference).Assembly);
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMQSettings = configuration.GetSection("RabbitMQSettings");
                var host = rabbitMQSettings["Host"];
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
                cfg.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
            });
        });


        //Configura JWT
        var jwtConfig = configuration.GetSection("jwtConfig");
        var secretKey = jwtConfig["secret"];
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig["validIssuer"],
                ValidAudience = jwtConfig["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}