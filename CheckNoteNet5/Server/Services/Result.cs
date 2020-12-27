using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckNoteNet5.Server.Services
{
    // look into ActionResult<T> !
    public class Result
    {
        public int StatusCode { get; init; } = StatusCodes.Status200OK;

        public static implicit operator ActionResult(Result r) => new StatusCodeResult(r.StatusCode);

        public static Result Ok() => new Result();
        // somehow merge these two:
        public static ErrorResult<object> Error<E>() where E : Error, new() => new ErrorResult<object>(new E());
        public static ErrorResult<object> Error<E>(string message) where E : Error, new() => new ErrorResult<object>(new E { Message = message });
    }

    public class Result<T> : Result
    {
        public T Value { get; init; }

        public Result(T value = default) => Value = value;

        public static implicit operator ActionResult<T>(Result<T> r) 
            => new ObjectResult(r.Value)
            {
                StatusCode = r.StatusCode
            };

        public bool IsOk { get => this is not ErrorResult<T>; }

        public Result<T> Ok(T value) => new Result<T>(value);
        public new ErrorResult<T> Error<E>() where E : Error, new() => new ErrorResult<T>(new E());
        public new ErrorResult<T> Error<E>(string message) where E : Error, new() => new ErrorResult<T>(new E { Message = message });
        public static ErrorResult<T> MakeError<E>() where E : Error, new() => new Result<T>().Error<E>();
        public static ErrorResult<T> MakeError<E>(string message) where E : Error, new() => new Result<T>().Error<E>(message);
    }

    public class ErrorResult<T> : Result<T>
    {
        public readonly Error error = new Error();
        public new int StatusCode { get => error.StatusCode; }

        public ErrorResult(Error error) => this.error = error;

        public static implicit operator ActionResult(ErrorResult<T> er)
        {
            // later add support for displayable errors (so we can have an errorCode but if no message then return nothing)
            if (er.error.Message != null || er.error.ErrorCode != default) return new ObjectResult(er.error);

            return new StatusCodeResult(er.StatusCode);
        }
    }
}
