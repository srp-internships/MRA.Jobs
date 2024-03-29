﻿namespace MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

public class RemoveTagsFromVacancyCommand : IRequest<List<string>>
{
    public string[] Tags { get; set; }
    public Guid VacancyId { get; set; }
}