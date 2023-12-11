namespace Services.Common.Models;

public interface IApiResponse<T>
{
    T Data { get; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
    bool Succeeded { get; }
}

public class ApiResponse<T> : IApiResponse<T>
{
    public ApiResponse()
    { }

    public ApiResponse(T data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }

    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
}