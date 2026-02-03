using FluentValidation;

namespace TreeWeb.Filters
{
    public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var arg = context.Arguments.FirstOrDefault(x => x is T) as T;
            if (arg is not null)
            {
                var result = await validator.ValidateAsync(arg);
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());
            }
            return await next(context);
        }
    }
}
