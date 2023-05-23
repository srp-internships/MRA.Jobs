using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyByIdQueryValidator : AbstractValidator<InternshipVacancyResponce>
{
    public GetInternshipVacancyByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
