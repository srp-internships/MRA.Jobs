namespace MRA.Jobs.Application.Contracts.Dtos;
public class EducationDetailDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Major { get; set; }
    public string UniversityName { get; set; }
    public string CertificateDegreeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
