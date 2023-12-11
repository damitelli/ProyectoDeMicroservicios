namespace PublicAPI.Controller;

[Route("v1/user-favorite-list")]
[ApiController]
public class UserFavoriteListController : BaseApiController
{
    public UserFavoriteListController(
        IMediator mediator,
        ILoggerService loggerService) : base(mediator, loggerService) { }


    /// <Summary>
    ///Retorna una collección de solo lectura de todas las listas de favoritos.
    /// </Summary>
    [HttpGet("all")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IApiResponse<IReadOnlyCollection<UserFavoriteListDTO>>>> GetAllUserFavoriteListAsync(
        CancellationToken cancellationToken = new()) =>
         Ok(await _mediator.Send(new GetAllUserFavoriteListQuery(), cancellationToken));


    /// <Summary>
    /// Obtiene una lista de favoritos de usuario filtrada por Id.
    /// </Summary>
    /// <param name="Id">El Id de la lista de favoritos de usuario buscar.</param>
    [HttpGet("{Id}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetUserFavoriteListByIdAsync(
        Guid Id,
        CancellationToken cancellationToken = new()) =>
         Ok(await _mediator.Send(new GetUserFavoriteListByIdQuery(Id), cancellationToken));


    /// <Summary>
    /// Obtiene una lista de favoritos de usuario filtrada por Id de usuario.
    /// </Summary>
    /// <param name="userId">El Id del usuario al que le pertenece la lista de favoritos a buscar.
    /// </param>
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetUserFavoriteListByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = new()) =>
         Ok(await _mediator.Send(new GetUserFavoriteListByUserIdQuery(userId), cancellationToken));


    /// <Summary>
    /// Crea una lista de favoritos para un usuario especificado por Id.
    /// </Summary>
    /// <param name= "userId">El Id del usuario al que le pertenece la lista de favoritos a crear.
    [HttpPost("create/{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateUserFavoriteListAsync(
        Guid userId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(new CreateUserFavoriteListCommand(userId), cancellationToken);
        return !result.Succeeded ?
                BadRequest(result) :
                CreatedAtAction(
                "CreateUserFavoriteList", result.Message = "Lista de favoritos creada con éxito.", result);
    }


    /// <Summary>
    /// Se agregará a la lista de favoritos de usario que coincida con el Id presente en la petición, un item favorito que coincida con el Id en el query.
    /// </Summary>
    /// <param name="Id">El Id de la lista de favoritos de usuario a la que se le agregará el item favorito.</param>
    /// <param name="itemId">El Id del item favorito a ser agregado.</param>
    [HttpPut("{Id}/additem/{itemId}")]
    [Authorize]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddFavoriteItemToFavoriteListAsync(
        Guid Id,
        Guid itemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(
            new AddFavoriteItemToUserFavoriteListCommand(Id, itemId), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Se agregó el Item favorito a la lista de favoritos con éxito.";
        return Ok(result);
    }


    /// <Summary>
    /// Se removerá de la lista de favoritos de usuario que coincida con el Id presente en la petición, un item favorito que coincida con el Id en el query.
    /// </Summary>
    /// <param name="userId">El Id de la lista de favoritos de usario que se le removerá el item favorito.</param>
    /// <param name="itemId">El Id del item favorito a ser removido.</param>
    [HttpPut("{userId}/removeitem/{itemId}")]
    [Authorize]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> RemoveItemFromUserFavoriteList(
        Guid userId,
        Guid ItemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(
            new RemoveFavoriteItemFromFavoriteListCommand(userId, ItemId), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Item favorito eliminado de la lista de favoritos con éxito.";
        return Ok(result);
    }


    /// <Summary>
    /// Se removerá de la lista de favoritos del usuario actual, un item favorito que coincida con el Id en el query.
    /// </Summary>
    /// <param name="itemId">El Id del item favorito a ser removido.</param>
    [HttpPut("removeitem/{itemId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> RemoveItemFromCurrentUserFavoriteList(
        Guid ItemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(
            new RemoveFavoriteItemFromCurrentUserFavoriteListCommand(ItemId), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Item favorito eliminado de la lista de favoritos con éxito.";
        return Ok(result);
    }
}