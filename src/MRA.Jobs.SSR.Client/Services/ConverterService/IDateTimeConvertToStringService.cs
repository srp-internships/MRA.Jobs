namespace MRA.Jobs.SSR.Client.Services.ConverterService;

public interface IDateTimeConvertToStringService
{
    string GetDisplayPostedDate(DateTime publishDate);
    (string DisplayDate, string Color) GetDeadlineOrEndDateDisplayDate(DateTime value);
}