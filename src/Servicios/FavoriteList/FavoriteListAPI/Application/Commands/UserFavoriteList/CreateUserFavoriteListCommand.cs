namespace Application.Commands.UserFavoritelist;

public record CreateUserFavoriteListCommand(Guid userId) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class CreateUserFavoriteListCommandHandler : IHandlerWrapper<CreateUserFavoriteListCommand, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IMapper _mapper;

    public CreateUserFavoriteListCommandHandler(IUserFavoriteListService userFavoriteListService, IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        CreateUserFavoriteListCommand request,
        CancellationToken cancellationToken)
    {
        var existingUserFavoriteList = await _userFavoriteListService.
        GetUserFavoriteListByUserIdAsync(request.userId);

        if (existingUserFavoriteList != null)
            throw new InvalidOperationException($"El Usuario Con Id:{request.userId} ya poseé una lista de favoritos ");

        var userFavoriteList = new UserFavoriteList
        {
            Id = Guid.NewGuid(),
            UserId = request.userId
        };
        await _userFavoriteListService.CreateUserFavoriteListAsync(userFavoriteList);

        return new ApiResponse<UserFavoriteListDTO>(_mapper.Map<UserFavoriteListDTO>(userFavoriteList));
    }
}
