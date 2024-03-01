using MRA.BlazorComponents.HttpClient.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.VacanciesServices.Training;

public interface ITrainingService
{
    Task<ApiResponse<string>> Create();
    Task<ApiResponse> Update(string slug);
    Task<ApiResponse> Delete(string slug);
    Task<PagedList<TrainingVacancyListDto>> GetAll(GetTrainingsQueryOptions queryOptions);
    Task<TrainingVacancyDetailedResponse> GetBySlug(string slug);
    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }

    Task ChangeNoteAsync(TrainingVacancyListDto vacancy);
}
