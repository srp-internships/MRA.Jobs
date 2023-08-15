using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Domain.Entities.ApplicantDetails;
public class UserExperience
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string JobLocationCity { get; set; }
    public string JobLocationCountry { get; set; }
    public string JobDescription { get; set; }
}
