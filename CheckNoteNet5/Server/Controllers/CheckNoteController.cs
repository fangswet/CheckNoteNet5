using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNoteNet5.Server.Controllers
{
    public class CheckNoteController : ControllerBase
    {
        protected static async Task<ActionResult> ServiceAction(Task<ServiceResult> sr) => (await sr).MapToAction();
        protected static async Task<ActionResult<T>> ServiceAction<T>(Task<ServiceResult<T>> sr) => (await sr).MapToAction();
        protected static async Task<ActionResult<T>> ServiceAction<T>(Task<ServiceError<T>> se) => (await se).MapToAction();
    }
}
