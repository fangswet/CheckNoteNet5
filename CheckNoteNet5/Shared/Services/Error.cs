using Microsoft.AspNetCore.Http;
using System;

namespace CheckNoteNet5.Shared.Services
{
    public class Error
    {
        public int ErrorCode { get; init; }
        public int StatusCode { get; init; }
        public string Message { get; init; }

        public Error(int errorCode = default, string message = null, int statusCode = StatusCodes.Status500InternalServerError)
        {
            ErrorCode = errorCode;
            Message = message;
            StatusCode = statusCode;
        }

        public Error(int errorCode, int statusCode)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }

    public class DevError : Error
    {
        public DevError() : base(ErrorCodes.Dev)
        { }
    }

    public class NotFoundError : Error
    {
        public NotFoundError() : base(default, StatusCodes.Status404NotFound)
        { }
    }

    public class TeapotError : Error
    {
        public TeapotError() : base(default, "test", StatusCodes.Status418ImATeapot)
        { }
    }

    public class UnauthorizedError : Error
    {
        public UnauthorizedError() : base(default, StatusCodes.Status401Unauthorized)
        { }
    }

    public class ConflictError : Error
    {
        public ConflictError() : base(default, StatusCodes.Status409Conflict)
        { }
    }

    public static class ErrorTypes
    {
        public static readonly Type Dev = typeof(DevError);
        public static readonly Type NotFound = typeof(NotFoundError);
    }
}
