using Microsoft.AspNetCore.Http;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceResult
    {
        public int StatusCode { get; init; } = StatusCodes.Status200OK;

        //public static implicit operator ActionResult(Result r) => new StatusCodeResult(r.StatusCode);

        public static ServiceResult Ok() => new ServiceResult();
        public static ServiceError<object> Error<E>() where E : Error, new() => new ServiceError<object>(new E());
        public static ServiceError<object> Error<E>(string message) where E : Error, new() => new ServiceError<object>(new E { Message = message });
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Value { get; init; }

        public ServiceResult(T value = default) => Value = value;

        //public static implicit operator ActionResult<T>(Result<T> r)
        //    => new ObjectResult(r.Value)
        //    {
        //        StatusCode = r.StatusCode
        //    };

        public bool IsOk { get => this is not ServiceError<T>; }

        public ServiceResult<T> Ok(T value) => new ServiceResult<T>(value);
        public static ServiceResult<V> Ok<V>(V value) => new ServiceResult<V>(value);
        public new ServiceError<T> Error<E>() where E : Error, new() => new ServiceError<T>(new E());
        public new ServiceError<T> Error<E>(string message) where E : Error, new() => new ServiceError<T>(new E { Message = message });
        public static ServiceError<T> MakeError<E>() where E : Error, new() => new ServiceResult<T>().Error<E>();
        public static ServiceError<T> MakeError<E>(string message) where E : Error, new() => new ServiceResult<T>().Error<E>(message);
    }

    public class ServiceError<T> : ServiceResult<T>
    {
        public readonly Error error = new Error();
        public new int StatusCode { get => error.StatusCode; }

        public ServiceError(Error error) => this.error = error;

        //public static implicit operator ActionResult(ErrorResult<T> er)
        //{
        //    if (er.error.Message != null || er.error.ErrorCode != default) return new ObjectResult(er.error);

        //    return new StatusCodeResult(er.StatusCode);
        //}
    }
}
