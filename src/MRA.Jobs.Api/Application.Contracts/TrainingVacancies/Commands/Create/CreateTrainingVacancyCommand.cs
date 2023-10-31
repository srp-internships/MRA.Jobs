using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;

public class CreateTrainingVacancyCommand : IRequest<string>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? EndDate { get; set; }

    public Guid CategoryId { get; set; }
    public int Duration { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public int Fees { get; set; }
}