using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.Applicant.Responses;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features;
public class MappingConfiguration
{
    public static void ConfigureUserMap<TSource, TDestination>(Profile profile)
    {
        if (typeof(TSource) == typeof(UserTimelineEvent) && typeof(TDestination) == typeof(TimeLineDetailsDto))
        {
            profile.CreateMap<UserTimelineEvent, TimeLineDetailsDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Avatar));
        }
        else if (typeof(TSource) == typeof(Tag) && typeof(TDestination) == typeof(TagDto))
        {
            profile.CreateMap<Tag, TagDto>();
        }
    }
    public static void ConfigureVacancyMap<TSource, TDestination>(Profile profile)
{
    if (typeof(TSource) == typeof(VacancyTimelineEvent) && typeof(TDestination) == typeof(TimeLineDetailsDto))
    {
            profile.CreateMap<VacancyTimelineEvent, TimeLineDetailsDto>();
           
    }
    else if (typeof(TSource) == typeof(Tag) && typeof(TDestination) == typeof(TagDto))
    {
        profile.CreateMap<Tag, TagDto>();
    }
}

}

