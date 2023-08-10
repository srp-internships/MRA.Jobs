using MRA.JobsTemp.Application.Common.Interfaces;

namespace MRA.JobsTemp.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
