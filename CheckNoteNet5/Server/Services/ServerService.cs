using CheckNoteNet5.Shared.Services;

namespace CheckNoteNet5.Server.Services
{
    public class ServerService
    {
        protected ServiceResult Ok() => ServiceResult.MakeOk();
        protected ServiceResult<T> Ok<T>(T value) => ServiceResult.MakeOk(value);
        protected ServiceResult NotFound() => ServiceResult.MakeError<NotFoundError>();
        protected ServiceResult<T> NotFound<T>() => ServiceResult<T>.MakeError<NotFoundError>();
        protected ServiceResult Unauthorized() => ServiceResult.MakeError<UnauthorizedError>();
        protected ServiceResult<T> Unauthorized<T>() => ServiceResult<T>.MakeError<UnauthorizedError>();
        protected ServiceResult Conflict() => ServiceResult.MakeError<ConflictError>();
        protected ServiceResult<T> Conflict<T>() => ServiceResult<T>.MakeError<ConflictError>();
        protected ServiceResult BadRequest() => ServiceResult.MakeError<BadRequestError>();
        protected ServiceResult<T> BadRequest<T>() => ServiceResult<T>.MakeError<BadRequestError>();
        protected ServiceResult<T> NullCheck<T>(T value) where T : class => ServiceResult.NullCheck(value);
    }
}
