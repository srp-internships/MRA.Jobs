﻿using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Domain.Entities;
using static Sieve.Extensions.MethodInfoExtended;
﻿using MRA.Jobs.Application.Contracts.Converter.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
public class TrainingVacancyListDTO
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
 
}
