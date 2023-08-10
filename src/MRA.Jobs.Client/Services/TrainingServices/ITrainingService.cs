using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public interface ITrainingService
{
    Task<List<TrainingVacancyListDTO>> GetAll();
    Task<TrainingVacancyDetailedResponce> GetById(Guid id);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(Guid id);
    Task Delete(Guid id);

    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }

}
