using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.Identity.Events;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.Applicants;

public class ApplicantProfile : Profile
{
    public ApplicantProfile()
    {
        CreateMap<CreateApplicantCommand, Applicant>();
        CreateMap<UpdateApplicantCommand, Applicant>();
        CreateMap<NewIdentityRegisteredEvent, Applicant>();
        CreateMap<Applicant, ApplicantListDto>();
        CreateMap<Applicant, ApplicantDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));
        MappingConfiguration.ConfigureUserMap<UserTimelineEvent, TimeLineDetailsDto>(this);
        MappingConfiguration.ConfigureUserMap<Tag, TagDto>(this);
        CreateMap<ApplicantSocialMedia, ApplicantSocialMediaDto>();
    }
}