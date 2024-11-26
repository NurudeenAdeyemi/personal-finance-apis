namespace Application.Exceptions;

public class CustomException : Exception
{
    public int HttpStatusCode { get; init; }
    public object? Error { get; init; }
    public string ErrorCode { get; init; } = default!;

    public CustomException(string message, string errorCode, int statusCode, object? error = null) : base(message)
    {
        HttpStatusCode = statusCode;
        Error = error;
        ErrorCode = errorCode;
    }

    public CustomException(string message, Exception innerException) : base(message, innerException)
    { }

    protected CustomException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }
}