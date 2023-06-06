using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{
    Task<List<InternshipVacancyListResponce>> GetAll();
    Task<InternshipVacancyResponce> GetById(Guid id);

    Task<HttpResponseMessage> Create(CreateInternshipVacancyCommand createCommand);
    Task Update(UpdateInternshipVacancyCommand updateCommand);
    Task Delete(Guid id);
    Task<List<CategoryResponse>> GetAllCategory();

    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
    List<CategoryResponse> Categories { get; set; }
}
