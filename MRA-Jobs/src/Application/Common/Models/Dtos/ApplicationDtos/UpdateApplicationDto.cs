using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Application.Common.Models.Dtos.ApplicationDtos;
public class UpdateApplicationDto
{
    public long Id { get; set; }
    public long ApplicantId { get; set; }
    public long VacancyId { get; set; }
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public long StatusId { get; set; }
}
