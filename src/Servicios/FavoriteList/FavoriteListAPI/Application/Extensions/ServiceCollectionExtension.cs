namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
        services.AddValidatorsFromAssemblyContaining<CreateFavoriteItemDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateFavoriteItemDTOValidator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        //Configura Logger
        services.AddScoped<ILoggerService, LoggerService>();
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("FavoriteListAPI init main");


        //Configure Mappings
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        var mapperConfig = new MapperConfiguration(map =>
        {
            map.AddProfile<UserFavoriteListMappingProfile>();
            map.AddProfile<FavoriteItemMappingProfile>();
            map.AddProfile<CreateFavoriteItemMappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());


        //Configura Swagger
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Favorite List API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor ingrese un token valido.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}