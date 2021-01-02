using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Services.Extensions
{
    // important asp adds a default error message when there is an error and no value
    public static class ServiceResultExtensions
    {
        public static ActionResult MapToAction(this Error e) => new ObjectResult(e.Message) { StatusCode = (int)e.StatusCode };
        public static ActionResult MapToAction(this ServiceResult sr) =>
            sr.IsOk
            ? new StatusCodeResult((int)sr.statusCode)
            : sr.error.MapToAction();

        public static ActionResult<T> MapToAction<T>(this ServiceResult<T> sr)
            => sr.IsOk
            ? sr.hasValue
                ? new ObjectResult(sr.Value) { StatusCode = (int)sr.statusCode }
                : new StatusCodeResult((int)sr.statusCode)
            : sr.error.MapToAction();

        public static async Task<ActionResult> MapToAction(this Task<ServiceResult> sr) => (await sr).MapToAction();
        public static async Task<ActionResult<T>> MapToAction<T>(this Task<ServiceResult<T>> sr) => (await sr).MapToAction();
    }
}
