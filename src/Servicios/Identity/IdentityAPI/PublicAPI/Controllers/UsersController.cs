namespace PublicAPI.Controllers;

[Route("v1/users")]
[ApiController]
public class UsersController : BaseApiController
{
    private readonly IValidator<UpdateUserInfoRequest> _updateValidator;
    public UsersController(
        IMediator mediator,
        ILoggerService logger,
        IValidator<UpdateUserInfoRequest> updateValidator) : base(mediator, logger) =>
            _updateValidator = updateValidator;



    /// <summary>
    /// Retorna una lista de solo lectura de todos los usuarios registrados.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<IApiResponse<IReadOnlyList<UserDTO>>>> GetAllUsersAsync(
        CancellationToken cancellationToken = new()) =>
            Ok(await _mediator.Send(new GetAllUsersQuery(), cancellationToken));


    /// <summary>
    ///  Obtiene un usuario según Id.
    /// </summary>
    /// <param name="userId">El Id del usuario a buscar.</param>
    [HttpGet("{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = new()) =>
            Ok(await _mediator.Send(new GetUserQuery(userId), cancellationToken));


    /// <summary>
    ///  Obtiene una lista de solo lectura de todos los refresh token pertenecientes
    ///  a un usuario según su Id.
    /// </summary>
    /// <param name="userId">El id del usuario del cual se debe obtener la lista de
    /// los refreshToken.</param>
    [HttpGet("{userId}/refresh-tokens-by-user-id")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> GetRefreshTokensByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = new()) =>
            Ok(await _mediator.Send(new GetRefreshTokenByUserIdQuery(userId), cancellationToken));


    /// <summary>
    ///  Actualiza los datos de un usuario específico según los datos presentes en la petición.
    /// </summary>
    /// <param name="userId">El Id del usuario que debe actualizarse.</param>
    /// <param name="updatedInfo">Los datos presentes en la petición.</param>
    [HttpPut("update/{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateUserAsync(
        string userId,
        UpdateUserInfoRequest updatedInfo,
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

        var result = await _mediator.Send(new UpdateUserInfoCommand(userId, updatedInfo), cancellationToken);

        if (!result.Succeeded)
            return BadRequest(result);

        result.Message = "Usuario actualizado con éxito.";
        return Ok(result);
    }
}
