﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

public class TrainingVacancyListDto
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public Guid CategoryId { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }

    public int Duration { get; set; }
    public int Fees { get; set; }
    public string Slug { get; set; }
}