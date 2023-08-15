using MRA.Jobs.Application.Contracts.Applications.Queries;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationById;
public class GetApplicationBySlugQueryValidator : AbstractValidator<GetBySlugApplicationQuery>
{
    public GetApplicationBySlugQueryValidator()
    {
        RuleFor(v => v.Slug).NotEmpty();
    }
}
