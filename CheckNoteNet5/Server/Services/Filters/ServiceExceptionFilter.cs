using CheckNoteNet5.Server.Services.Extensions;
using CheckNoteNet5.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CheckNoteNet5.Server.Services.Filters
{
    public class ServiceExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ServiceException exception)
            {
                context.Result = exception.Error.MapToAction();
                context.ExceptionHandled = true;
            }
        }
    }
}
