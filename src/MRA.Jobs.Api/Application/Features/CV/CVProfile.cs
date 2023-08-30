using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.CV;
public class CVProfile : Profile
{
    public CVProfile()
    {
        CreateMap<EducationDetail, EducationDetailDto>();
        CreateMap<ExperienceDetail, ExperienceDetailDto>();
        CreateMap<Skill, SkillDto>();
    }
}
