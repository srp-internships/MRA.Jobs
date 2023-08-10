﻿using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{
    Task<List<InternshipVacancyListResponce>> GetAll();
    Task<InternshipVacancyResponce> GetById(Guid id);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(Guid id);
    Task Delete(Guid id);

    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
}
