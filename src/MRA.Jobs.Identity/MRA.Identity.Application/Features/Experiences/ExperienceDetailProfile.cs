using AutoMapper;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Application.Contract.Experiences.Commands.Update;
using MRA.Identity.Application.Contract.Experiences.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Experiences;
public class ExperienceDetailProfile : Profile
{
    public ExperienceDetailProfile()
    {
        CreateMap<CreateExperienceDetailCommand, ExperienceDetail>();
        CreateMap<UpdateExperienceDetailCommand, ExperienceDetail>();
        CreateMap<ExperienceDetail, UserExperienceResponse>();
    }
}
