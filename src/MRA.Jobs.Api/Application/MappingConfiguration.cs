using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application;
public class MappingConfiguration
{
    public static void ConfigureUserMap<TSource, TDestination>(Profile profile)
    {
        if (typeof(TSource) == typeof(ApplicationTimelineEvent) && typeof(TDestination) == typeof(TimeLineDetailsDto))
        {
            profile.CreateMap<ApplicationTimelineEvent, TimeLineDetailsDto>();
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

