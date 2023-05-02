using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Application.Common.Models.Dtos.CategoryDtos;
public class GetCategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<JobVacancy> JobVacancies { get; set; }
    public List<EducationVacancy> EducationVacancies { get; set; }
}
