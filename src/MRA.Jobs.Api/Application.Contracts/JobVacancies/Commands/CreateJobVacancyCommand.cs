﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using Newtonsoft.Json;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class CreateJobVacancyCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public IEnumerable<JobQuestionDto> VacancyQuestions { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}