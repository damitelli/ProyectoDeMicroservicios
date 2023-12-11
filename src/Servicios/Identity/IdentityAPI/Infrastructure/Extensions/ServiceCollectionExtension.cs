namespace Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Configura PostgreSQLContext
        services.AddDbContextPool<ApplicationDbContext>(opts =>
                opts.UseNpgsql(configuration.GetConnectionString("IdentityApiDatabase")));


        //Configura Servicios
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();


        //Configura Identity
        var builder = services.AddIdentity<ApplicationUser, IdentityRole>(o =>
        {
            o.SignIn.RequireConfirmedEmail = false;
            o.User.RequireUniqueEmail = false;
            o.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.User.RequireUniqueEmail = true;
            o.Password.RequiredUniqueChars = 1;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


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