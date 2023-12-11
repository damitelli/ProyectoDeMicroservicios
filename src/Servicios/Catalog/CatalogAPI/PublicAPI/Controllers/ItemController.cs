namespace PublicAPI.Controller;

[Route("v1/items")]
[ApiController]
public class ItemController : BaseApiController
{
    private readonly IValidator<CreateItemRequest> _createValidator;

    private readonly IValidator<UpdateItemRequest> _updateValidator;

    public ItemController(
        IMediator mediator,
        ILoggerService loggerService,
        IValidator<CreateItemRequest> createValidator,
        IValidator<UpdateItemRequest> updateValidator) : base(mediator, loggerService)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <Summary>
    ///Retorna una collección de solo lectura de todos los items.
    /// </Summary>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IApiResponse<IReadOnlyCollection<ItemDTO>>>> GetAllItemsAsync(CancellationToken cancellationToken = new()) =>
        Ok(await _mediator.Send(new GetAllItemsQuery(), cancellationToken));


    /// <Summary>
    /// Obtiene un item filtrado por id.
    /// </Summary>
    /// <param name= "itemId"> El id del item a buscar.</param>
    [HttpGet("{itemId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetItemByIdAsync(
        Guid itemId,
        CancellationToken cancellationToken = new()) =>
            Ok(await _mediator.Send(new GetItemByIdQuery(itemId), cancellationToken));


    /// <Summary>
    /// Crea un item a partir de los datos presentes en la petición.
    /// </Summary>
    /// <param name="createInfo">lost datos presentes en la petición.</param>
    [HttpPost("create")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateItemAsync(
        [FromBody] CreateItemRequest createInfo,
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

        var result = await _mediator.Send(new CreateItemCommand(createInfo), cancellationToken);
        return !result.Succeeded ?
                BadRequest(result) :
                CreatedAtAction(
                "CreateItem", result.Message = "Item creado con éxito.", result);
    }


    /// <Summary>
    /// Actualiza los datos de un item específico según los datos presentes en la petición.
    /// </Summary>
    /// <param name="itemId">El Id del item que debe actualizarse.</param>
    /// <param name="updateInfo">lost datos presentes en la petición.</param>
    [HttpPut("update/{itemId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateItemAsync(
        Guid itemId,
        [FromBody] UpdateItemRequest updateInfo,
        CancellationToken cancellationToken = new())
    {
        var validationResult = await _updateValidator.ValidateAsync(updateInfo, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(
               validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                   g => g.Key,
                   g => g.Select(x => x.ErrorMessage).ToArray()
                ));

        var result = await _mediator.Send(new UpdateItemCommand(itemId, updateInfo), cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        result.Message = "Item actualizado con éxito.";
        return Ok(result);

    }

    /// <Summary>
    /// Se eliminara el item que coincida con el id presente en la petición.
    /// </Summary>
    /// <param name="itemId">El Id del item que debe eliminarse.</param>
    [HttpDelete("delete/{itemId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> DeleteItemAsync(
        Guid itemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(new DeleteItemCommand(itemId), cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        result.Message = "Item eliminado con éxito.";
        return Ok(result);
    }


    /// <Summary>
    /// Se agregará al item que coincida con el id presente en la petición, un item que coincida con el id en el query.
    /// </Summary>
    /// <param name="itemId">El Id del item a ser agregado.</param>
    [HttpPost("additemtolist/{itemId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> AddItemToFavoriteListAsync(
        Guid itemId,
        CancellationToken cancellationToken = new())
    {
        var result = await _mediator.Send(new AddItemToFavoriteListCommand(itemId), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "El item ha sido enviado a tu lista de favoritos exitosamente.";
        return Ok(result);
    }
}