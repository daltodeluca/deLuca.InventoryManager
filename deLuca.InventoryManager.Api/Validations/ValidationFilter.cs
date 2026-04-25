using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace deLuca.InventoryManager.Api.Validations;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService(typeof(IValidator<T>)) as IValidator<T>;
        if (validator is not null)
        {
            var arg = context.Arguments.FirstOrDefault(a => a is T) as T;
            if (arg is not null)
            {
                var result = await validator.ValidateAsync(arg);
                if (!result.IsValid)
                {
                    return Results.ValidationProblem(result.ToDictionary());
                }
            }
        }

        return await next(context);
    }
}
