using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckNoteNet5.Server.Services.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ActionResult MapToAction(this ServiceResult sr) => new StatusCodeResult(sr.StatusCode);

        public static ActionResult<T> MapToAction<T>(this ServiceResult<T> sr)
            => new ObjectResult(sr.Value)
            {
                StatusCode = sr.StatusCode
            };

        public static ActionResult<T> MapToAction<T>(this ServiceError<T> se)
        {
            if (se.error.Message != null || se.error.ErrorCode != default) return new ObjectResult(se.error);

            return new StatusCodeResult(se.StatusCode);
        }
    }
}
