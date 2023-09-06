
namespace MRA.Jobs.Application.Common.Exceptions;
public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> NotEmptyWithDefaultMessage<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("{PropertyName} should not be empty");
    }
}
