using System;
using System.Net;

namespace CheckNoteNet5.Shared.Services
{
    public enum ErrorCode
    {
        Dev = 1000
    }

    public class Error
    {
        public ErrorCode ErrorCode { get; init; }
        public HttpStatusCode StatusCode { get; init; }
        public string Message { get; init; }

        public Error(ErrorCode errorCode = default, string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            ErrorCode = errorCode;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class DevError : Error
    {
        public DevError() : base(ErrorCode.Dev)
        { }
    }

    public class NotFoundError : Error
    {
        public NotFoundError() : base(statusCode: HttpStatusCode.NotFound)
        { }
    }

    public class TeapotError : Error
    {
        public TeapotError() : base(default, "test", HttpStatusCode.SeeOther)
        { }
    }

    public class UnauthorizedError : Error
    {
        public UnauthorizedError() : base(statusCode: HttpStatusCode.Unauthorized)
        { }
    }

    public class ConflictError : Error
    {
        public ConflictError() : base(statusCode: HttpStatusCode.Conflict)
        { }
    }

    public class BadRequestError : Error
    {
        public BadRequestError() : base(statusCode: HttpStatusCode.BadRequest)
        { }
    }

    public static class ErrorTypes
    {
        public static readonly Type Dev = typeof(DevError);
        public static readonly Type NotFound = typeof(NotFoundError);
        public static readonly Type Unauthorized = typeof(UnauthorizedError);
        public static readonly Type Conflict = typeof(ConflictError);
        public static readonly Type BadRequest = typeof(BadRequestError);
    }
}
