using CroBooks.Shared.Enums;
using System.Runtime.Serialization;

namespace CroBooks.Shared.Exceptions;

[Serializable]
public class HttpResponseException : Exception
{
    public ErrorTypes? ErrorType;

    public HttpResponseException()
    {
    }

    public HttpResponseException(string message)
        : base(message)
    {
    }

    public HttpResponseException(string message, ErrorTypes errorType)
        : base(message)
    {
        ErrorType = errorType;
    }

    public HttpResponseException(string message, Exception innerException, ErrorTypes errorType)
        : base(message, innerException)
    {
        ErrorType = errorType;
    }

    protected HttpResponseException(SerializationInfo info, StreamingContext context, ErrorTypes errorType)
        : base(info, context)
    {
        ErrorType = errorType;
    }
}