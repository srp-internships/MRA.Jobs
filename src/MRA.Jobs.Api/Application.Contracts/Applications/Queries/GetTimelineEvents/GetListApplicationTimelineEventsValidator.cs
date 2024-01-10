namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetTimelineEvents;

public class GetListApplicationTimelineEventsQueryValidator : AbstractValidator<GetListApplicationTimelineEventsQuery>
{
    public GetListApplicationTimelineEventsQueryValidator()
    {
        RuleFor(x => x.ApplicationSlug).NotEmpty();
    }
}