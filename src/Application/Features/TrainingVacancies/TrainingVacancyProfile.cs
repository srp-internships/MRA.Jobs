using MRA.Jobs.Application.Contracts.TrainingModels.Commands;
using MRA.Jobs.Application.Contracts.TrainingModels.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies;
public class TrainingVacancyProfile : Profile
{
    public TrainingVacancyProfile()
    {
        CreateMap<TrainingVacancy, TrainingVacancyListDTO>();
        CreateMap<TrainingVacancy, TrainingModelDetailsDTO>();
        CreateMap<CreateTrainingVacancyCommand, TrainingVacancy>();
        CreateMap<UpdateTrainingVacancyCommand, TrainingVacancy>();
    }
}
