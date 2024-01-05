using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{

    Task<ApiResponse> Create();
    Task<ApiResponse> Update(string slug);
    Task<ApiResponse> Delete(string slug);
    Task<ApiResponse<List<InternshipVacancyListResponse>>> GetAll();
    Task<InternshipVacancyResponse> GetBySlug(string slug);
    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
}
