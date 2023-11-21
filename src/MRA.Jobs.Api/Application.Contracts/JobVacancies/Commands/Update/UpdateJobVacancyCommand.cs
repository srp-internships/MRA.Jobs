using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.Dtos;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;

public class UpdateJobVacancyCommand : IRequest<string>
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? PublishDate { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime? EndDate { get; set; }

    public Guid CategoryId { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public Dtos.Enums.ApplicationStatusDto.WorkSchedule WorkSchedule { get; set; }
    public IEnumerable<VacancyQuestionDto> VacancyQuestions { get; set; }
    public IEnumerable<VacancyTaskDto> VacancyTasks { get; set; }
}