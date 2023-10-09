using AutoMapper;
using MRA.Identity.Application.Contract.Experiences.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Experiences;
public class ExperienceDetailProfile : Profile
{
    public ExperienceDetailProfile()
    {
        CreateMap<CreateExperienceDetailCommand, ExperienceDetail>();
    }
}
