﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;

public class UpdateInternshipVacancyCommand : IRequest<string>
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime CreatedAt { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? EndDate { get; set; }

    public string Location { get; set; }
    public string RequiredSkills { get; set; }
    public Guid CategoryId { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? ApplicationDeadline { get; set; }

    public int Duration { get; set; }
    public int Stipend { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTaskDto> VacancyTasks { get; set; }
}