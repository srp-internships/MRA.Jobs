using MRA.Jobs.Application.Common.Interfaces;

namespace MRA.Jobs.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
