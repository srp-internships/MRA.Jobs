using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies;
public class TrainingVacancyProfile : Profile
{
    public TrainingVacancyProfile()
    {
        CreateMap<TrainingVacancy, TrainingVacancyListDTO>();
        CreateMap<TrainingVacancy, TrainingVacancyDetailedResponce>();
        CreateMap<CreateTrainingVacancyCommand, TrainingVacancy>();
        CreateMap<UpdateTrainingVacancyCommand, TrainingVacancy>();
    }
}
