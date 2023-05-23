namespace MRA.Jobs.Application.Common.Models;
public class EmailMessage
{
    public string To { get; set; }
    public string From { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
}
