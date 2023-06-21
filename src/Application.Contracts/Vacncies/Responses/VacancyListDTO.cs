using System.IO.Pipes;

namespace MRA.Jobs.Application.Contracts.Vacncies.Responses;
public class VacancyListDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    
    public VacancyType Discriminator { get; set; }
}


public enum VacancyType
{
    JobVacancy,
    InternshipVacancy,
    TrainingVacancy
}