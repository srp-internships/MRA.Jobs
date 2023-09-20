
namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
public class GetApplicationBySlugQueryValidator : AbstractValidator<GetBySlugApplicationQuery>
{
    public GetApplicationBySlugQueryValidator()
    {
        RuleFor(v => v.Slug).NotEmpty();
    }
}
