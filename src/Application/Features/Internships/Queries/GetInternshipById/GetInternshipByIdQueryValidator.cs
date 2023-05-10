using MRA.Jobs.Application.Contracts.Internships.Responses;

namespace MRA.Jobs.Application.Features.Internships.Queries.GetInternshipById;
public class GetInternshipByIdQueryValidator : AbstractValidator<InternshipDetailsDTO>
{
    public GetInternshipByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
