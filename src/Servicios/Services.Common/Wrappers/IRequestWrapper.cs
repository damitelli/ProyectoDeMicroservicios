namespace Services.Common.Wrappers;

///<Summary>
///Wrapper Interface para IRequest. Retorna IResponse{T}
///<Summary>
public interface IRequestWrapper<T> : IRequest<IApiResponse<T>> { }

/// <summary>
/// Wrapper Interface para IRequestHandler{TRequest,TResponse}. To Handle IRequestWrapper{T}
/// </summary>
public interface IHandlerWrapper<in TRequest, TResponse> :
    IRequestHandler<TRequest, IApiResponse<TResponse>> where TRequest : IRequestWrapper<TResponse>
{ }
