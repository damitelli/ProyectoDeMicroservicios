namespace PublicAPI.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly ILoggerService _logger;

    public BaseApiController(IMediator mediator, ILoggerService logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}