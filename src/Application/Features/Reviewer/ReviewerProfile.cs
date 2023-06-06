using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Contracts.Reviewer.Response;

namespace MRA.Jobs.Application.Features.Reviewer;
using Domain.Entities;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

public class ReviewerProfile : Profile
{
    public ReviewerProfile()
    {
        CreateMap<Reviewer, ReviewerListDto>();
        CreateMap<Reviewer, ReviewerDetailsDto>()
              .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));
        CreateMap<CreateReviewerCommand, Reviewer>();
        CreateMap<UpdateReviewerCommand, Reviewer>();
        MappingConfiguration.ConfigureUserMap<UserTimelineEvent, TimeLineDetailsDto>(this);
        MappingConfiguration.ConfigureUserMap<Tag, TagDto>(this);
    }
}