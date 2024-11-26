using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Validations;
public static class Extensions
{
    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}