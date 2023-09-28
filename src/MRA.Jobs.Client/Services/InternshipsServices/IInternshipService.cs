using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(string slug);
    Task Delete(string slug);
    Task<List<InternshipVacancyListResponse>> GetAll();
    Task<InternshipVacancyResponse> GetBySlug(string slug);

    Task<PagedList<TrainingVacancyListDto>> GetByCategoryName(string slug);
    Task<PagedList<TrainingVacancyListDto>> SearchInternship(string searchInput);

    Task<PagedList<TrainingVacancyListDto>> GetAllSinceCheckDate();
    Task<PagedList<TrainingVacancyListDto>> GetByCategorySinceCheckDate(string slug);
    Task<PagedList<TrainingVacancyListDto>> SearchInternshipSinceSearchDate(string searchInput);
    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
}
