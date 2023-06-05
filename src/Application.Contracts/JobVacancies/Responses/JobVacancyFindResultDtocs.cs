﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;

public class JobVacancyFindResultDto
{
    public string Category { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; } 
    public string ShortDescription { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}