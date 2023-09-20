using System.Net.Http;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public interface ITrainingService
{
    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(string slug);
    Task Delete(string slug);


    Task<PagedList<TrainingVacancyListDto>> GetAll();
    Task<TrainingVacancyDetailedResponse> GetBySlug(string slug);
    Task<PagedList<TrainingVacancyListDto>> GetByCategoryName(string slug);
    Task<PagedList<TrainingVacancyListDto>> SearchTrainings(string searchInput);

    Task<PagedList<TrainingVacancyListDto>> GetAllSinceCheckDate();
    Task<PagedList<TrainingVacancyListDto>> GetByCategorySinceCheckDate(string slug);
    Task<PagedList<TrainingVacancyListDto>> SearchTrainingsSinceSearchDate(string searchInput);

    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }

}
