using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
