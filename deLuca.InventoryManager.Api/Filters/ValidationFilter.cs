using FluentValidation;

namespace deLuca.InventoryManager.Api.Filters;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null)
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();
            if (argument is not null)
            {
                var result = await validator.ValidateAsync(
                    argument, context.HttpContext.RequestAborted);

                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());
            }
        }

        return await next(context);
    }
}
