using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckNoteNet5.Server.Services.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ActionResult MapToAction(this ServiceResult sr) => new StatusCodeResult((int) sr.StatusCode);
        public static ActionResult<T> MapToAction<T>(this ServiceResult<T> sr)
            => new ObjectResult(sr.GetValue())
            {
                StatusCode = (int) sr.StatusCode
            };
    }
}
