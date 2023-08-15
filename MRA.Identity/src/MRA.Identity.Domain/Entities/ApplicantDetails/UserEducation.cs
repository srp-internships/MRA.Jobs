using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Domain.Entities.ApplicantDetails;
public class UserEducation
{
    public Guid Id { get; set; }
    public string CertificateDegree { get; set; }
    public string Major { get; set; }
    public string UniversityName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
