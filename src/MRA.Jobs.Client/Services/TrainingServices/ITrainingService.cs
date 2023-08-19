﻿using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public interface ITrainingService
{
    Task<PagedList<TrainingVacancyListDto>> GetAll();
    Task<TrainingVacancyDetailedResponse> GetBySlug(string slug);
    Task<TrainingVacancyDetailedResponse> GetBySlugSinceCheckDate(string slug);
    Task<List<TrainingCategoriesResponce>> GetCategories();
    Task<PagedList<TrainingVacancyListDto>> GetByCategoryName(string slug);
    Task<PagedList<TrainingVacancyListDto>> SearchTrainings(string searchInput);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(string slug);
    Task Delete(string slug);

    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }

}
