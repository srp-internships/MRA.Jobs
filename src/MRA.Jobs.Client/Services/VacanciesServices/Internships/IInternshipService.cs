using MRA.BlazorComponents.HttpClient.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Client.Services.VacanciesServices.Internships;

public interface IInternshipService
{
    Task<ApiResponse> Create();
    Task<ApiResponse> Update(string slug);
    Task<bool> Delete(string slug);
    Task<ApiResponse<PagedList<InternshipVacancyListResponse>>> GetAll();
    Task<InternshipVacancyResponse> GetBySlug(string slug);
    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
    Task ChangeNoteAsync(InternshipVacancyListResponse vacancy);
    
}