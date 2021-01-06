using System;
using System.Net;

namespace CheckNoteNet5.Shared.Services
{
    public class ServiceException : Exception
    {
        public ServiceError Error { get; init; }

        public ServiceException(string message = null) : base(message)
        { }
    }

    public class ServiceException<TError> : ServiceException where TError : ServiceError, new()
    {
        public ServiceException(string message = null) : base(message) => Error = new TError();
    }

    public class UnauthorizedException : ServiceException<UnauthorizedError>
    { }
}
