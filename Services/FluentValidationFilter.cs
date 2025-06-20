using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services
{

    //IAsyncActionFilter HTTP isteği bir controller action metoduna ulaşmadan önce çalışır.
    public class FluentValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var resultModel = ServiceResult.Fail(errors);
                context.Result = new BadRequestObjectResult(resultModel);
                return;
            }

            await next(); // Action metodunu çalıştırır
        }
    }
}
