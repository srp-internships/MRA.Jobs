using MediatR;
using MRA.Jobs.Application.Common.Converter;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.TrainingModels.Commands;
public class CreateTrainingModelCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime PublishDate { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public int Duration { get; set; }
    public int Fees { get; set; }
}
