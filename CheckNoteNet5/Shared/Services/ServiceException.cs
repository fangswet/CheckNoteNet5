using System;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceException : Exception
    {
        public ServiceError Error { get; init; }
        public ServiceException() { }
        public ServiceException(ServiceError error) => Error = error;
    }

    public class ServiceException<TError> : ServiceException where TError : ServiceError, new()
    {
        public ServiceException() => Error = new TError();
        public ServiceException(string message) => Error = new TError { Message = message };
    }

    public class UnauthorizedException : ServiceException<UnauthorizedError>
    { }
}
