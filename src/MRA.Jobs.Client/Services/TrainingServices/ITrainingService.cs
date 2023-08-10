using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public interface ITrainingService
{
    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }
    Task<List<TrainingVacancyListDto>> GetAll();
    Task<TrainingVacancyDetailedResponse> GetById(Guid id);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(Guid id);
    Task Delete(Guid id);
}