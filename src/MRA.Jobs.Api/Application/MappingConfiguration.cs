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
    }
    public static void ConfigureVacancyMap<TSource, TDestination>(Profile profile)
    {
        if (typeof(TSource) == typeof(VacancyTimelineEvent) && typeof(TDestination) == typeof(TimeLineDetailsDto))
        {
            profile.CreateMap<VacancyTimelineEvent, TimeLineDetailsDto>();
        }
    }

}

