﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
public class CreateInternshipVacancyCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime ApplicationDeadline { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
}
