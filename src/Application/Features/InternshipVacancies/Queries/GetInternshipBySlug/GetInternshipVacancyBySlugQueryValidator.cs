using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyBySlugQueryValidator : AbstractValidator<InternshipVacancyResponce>
{
    public GetInternshipVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}