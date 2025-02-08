using Microsoft.AspNetCore.Mvc.Filters;
using TABP.Controllers;

namespace TABP.Utilites
{
    public class LogFilter(ILogger<LogFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller.GetType() == typeof(AuthController))
            {
                logger.LogInformation(
                  "Executing {$ActionMethodName} on controller {$ControllerName}",
                  context.ActionDescriptor.DisplayName,
                  context.Controller);

                await next();

                logger.LogInformation(
                  "Action {$ActionMethodName} finished execution on controller {$ControllerName}",
                  context.ActionDescriptor.DisplayName,
                  context.Controller);

                return;
            }

            logger.LogInformation(
              "Executing {$ActionMethodName} on controller {$ControllerName}, with arguments {@ActionArguments}",
              context.ActionDescriptor.DisplayName,
              context.Controller);

            await next();

            logger.LogInformation(
              "Action {$ActionMethodName} finished execution on controller {$ControllerName}, with arguments {@ActionArguments}",
              context.ActionDescriptor.DisplayName,
              context.Controller);
        }
    }
}
