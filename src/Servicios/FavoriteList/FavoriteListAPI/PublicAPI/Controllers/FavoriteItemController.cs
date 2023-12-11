namespace PublicAPI.Controller;

[Route("v1/favorite-items")]
[ApiController]
public class FavoriteItemController : BaseApiController
{
    private readonly IValidator<CreateFavoriteItemRequest> _createValidator;

    private readonly IValidator<UpdateFavoriteItemRequest> _updateValidator;

    public FavoriteItemController(
        IMediator mediator,
        ILoggerService loggerService,
        IValidator<CreateFavoriteItemRequest> createValidator,
        IValidator<UpdateFavoriteItemRequest> updateValidator) : base(mediator, loggerService)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }


    /// <Summary>
    ///Retorna una collección de solo lectura de todos los items favoritos.
    /// </Summary>
    [HttpGet("all")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IApiResponse<IReadOnlyCollection<FavoriteItemDTO>>>> GetAllFavoriteItemAsync(CancellationToken cancellationToken = new()) =>
        Ok(await _mediator.Send(new GetAllFavoriteItemQuery(), cancellationToken));


    /// <Summary>
    /// Obtiene un item favorito filtrado por Id.
    /// </Summary>
    /// <param name= "itemId"> El Id del item favorito a buscar.</param>
    [HttpGet("{itemId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetFavoriteItemByIdAsync(
        Guid itemId,
        CancellationToken cancellationToken = new()) =>
            Ok(await _mediator.Send(new GetFavoriteItemByIdQuery(itemId), cancellationToken));


    /// <Summary>
    /// Crea un item favorito a partir de los datos presentes en la petición.
    /// </Summary>
    /// <param name="createInfo">lost datos presentes en la petición.</param>
    [HttpPost("create")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateFavoriteItemAsync(
        [FromBody] CreateFavoriteItemRequest createInfo,
        CancellationToken cancellationToken = new())
    {
        var validationResult = await _createValidator.ValidateAsync(createInfo, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(
               validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                   g => g.Key,
                   g => g.Select(x => x.ErrorMessage).ToArray()
                ));

        var result = await _mediator.Send(new CreateFavoriteItemCommand(createInfo), cancellationToken);
        return !result.Succeeded ?
                BadRequest(result) :
                CreatedAtAction(
                "CreateFavoriteItem", result.Message = "Item favorito creado con éxito", result);
    }


    /// <Summary>
    /// Actualiza los datos de un item favorito específico según los datos presentes en la petición.
    /// </Summary>
    /// <param name="ItemId">El Id del item favorito que debe actualizarse.</param>
    /// <param name="updatedInfo">lost datos presentes en la petición.</param>
    [HttpPut("update/{ItemId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateFavoriteItemAsync(
        Guid ItemId,
        [FromBody] UpdateFavoriteItemRequest updatedInfo,
        CancellationToken cancellationToken = new())
    {
        var validationResult = await _updateValidator.ValidateAsync(updatedInfo, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(
               validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                   g => g.Key,
                   g => g.Select(x => x.ErrorMessage).ToArray()
                ));

        var result = await _mediator.Send(
           new UpdateFavoriteItemCommand(ItemId, updatedInfo), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Item favorito actualizado con éxito.";
        return Ok(result);
    }


    /// <Summary>
    /// Se eliminara el item favorito que coincida con el Id presente en la petición.
    /// </Summary>
    /// <param name="ItemId">El Id del item favorito que debe eliminarse.</param>
    [HttpDelete("delete/{ItemId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> DeleteFavoriteItemAsync(
        Guid ItemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(new DeleteFavoriteItemCommand(ItemId), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Item favorito eliminado con éxito.";
        return Ok(result);
    }
}