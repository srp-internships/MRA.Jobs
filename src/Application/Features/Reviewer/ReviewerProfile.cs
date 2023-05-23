using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer;
using Domain.Entities;
using MRA.Jobs.Application.Contracts.Reviewer.Commands;

public class ReviewerProfile : Profile
{
    public ReviewerProfile()
    {
        CreateMap<Reviewer, ReviewerListDto>();
        CreateMap<Reviewer, ReviewerDetailsDto>();
        CreateMap<CreateReviewerCommand, Reviewer>();
        CreateMap<UpdateReviewerCommand, Reviewer>();
    }
}