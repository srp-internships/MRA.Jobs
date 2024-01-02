﻿using System.Net.Http;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public interface ITrainingService
{
    Task<ApiResponse> Create();
    Task<ApiResponse> Update(string slug);
    Task<ApiResponse> Delete(string slug);
    Task<ApiResponse<PagedList<TrainingVacancyListDto>>> GetAll(GetTrainingVacancyBySlugQuery getTrainingVacancyBySlug);
    Task<TrainingVacancyDetailedResponse> GetBySlug(string slug, GetTrainingVacancyBySlugQuery getTrainingVacancyBySlug);
    CreateTrainingVacancyCommand createCommand { get; set; }
    UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    DeleteTrainingVacancyCommand DeleteCommand { get; set; }

}
