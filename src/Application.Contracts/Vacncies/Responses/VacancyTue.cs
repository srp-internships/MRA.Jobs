using System.IO.Pipes;

namespace MRA.Jobs.Application.Contracts.Vacncies.Responses;
public class VacancyListDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
 
}

