namespace Services.Common.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public IdentityService(IHttpContextAccessor httpContextAccesor) =>
    _httpContextAccesor = httpContextAccesor;

    public string GetUserId()
    {
        var userId = _httpContextAccesor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException("No se encuentra el Token.");

        return userId;
    }
}