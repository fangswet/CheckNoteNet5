using System.Net;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceError
    {
        public ErrorCode ErrorCode { get; init; }
        public HttpStatusCode StatusCode { get; init; }
        public string Message { get; init; }

        public ServiceError(ErrorCode errorCode = default, string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            ErrorCode = errorCode;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class DeveloperError : ServiceError
    {
        public DeveloperError() : base(ErrorCode.Developer)
        { }
    }

    public class NotFoundError : ServiceError
    {
        public NotFoundError() : base(statusCode: HttpStatusCode.NotFound)
        { }
    }

    public class TeapotError : ServiceError
    {
        public TeapotError() : base(default, "test", HttpStatusCode.SeeOther)
        { }
    }

    public class UnauthorizedError : ServiceError
    {
        public UnauthorizedError() : base(statusCode: HttpStatusCode.Unauthorized)
        { }
    }

    public class ConflictError : ServiceError
    {
        public ConflictError() : base(statusCode: HttpStatusCode.Conflict)
        { }
    }

    public class BadRequestError : ServiceError
    {
        public BadRequestError() : base(statusCode: HttpStatusCode.BadRequest)
        { }
    }
}
