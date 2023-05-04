using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Domain.Entities
{
    public class TestResult : BaseAuditableEntity
    {
        public long ApplicationId { get; set; }
        public DateTime CompletedAt { get; set; }
        public bool Passed { get; set; }
        public int Score { get; set; }
        public long TestId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Test Test { get; set; }
        public Application Application { get; set; }


    }
}
