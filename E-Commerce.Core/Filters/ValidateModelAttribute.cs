using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new Response<object>
                {
                    Errors = context.ModelState
                        .Where(x => x.Value.Errors.Any())
                        .SelectMany(x => x.Value.Errors.Select(e => new Error
                        {
                            Code = 400,
                            Message = $"Field: {x.Key} - {e.ErrorMessage}"
                        }))
                        .ToList()
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}