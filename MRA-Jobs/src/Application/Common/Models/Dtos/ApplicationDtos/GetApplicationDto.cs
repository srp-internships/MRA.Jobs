using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Application.Common.Models.Dtos.ApplicationDtos;
public class GetApplicationDto
{
    public long Id { get; set;}
    public Applicant Applicant { get; set;}
    public Vacancy Vacancy { get; set;}
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public Status Status { get; set; }
}
