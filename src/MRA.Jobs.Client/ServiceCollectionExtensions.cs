using FluentValidation;
using System.Reflection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var validatorType = typeof(IValidator<>);
        var validatorTypes = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == validatorType));

        foreach (var validator in validatorTypes)
        {
            var requestType = validator.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)
                .Select(i => i.GetGenericArguments()[0])
                .First();

            var validatorInterface = validatorType.MakeGenericType(requestType);
            services.AddTransient(validatorInterface, validator);
        }

        return services;
    }
}
