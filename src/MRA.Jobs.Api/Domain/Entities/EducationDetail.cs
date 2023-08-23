using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities;
public class EducationDetail
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Major { get; set; }
    public string UniversityName { get; set; }
    public string CertificateDegreeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
