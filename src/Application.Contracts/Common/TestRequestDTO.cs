using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.Contracts.Common;
public class TestRequestDTO
{
    public long NumberOfQuestion { get; set; }
    public List<string> Categories { get; set; }
}
