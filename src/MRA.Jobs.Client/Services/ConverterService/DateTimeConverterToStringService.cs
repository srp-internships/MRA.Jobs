using MRA.Jobs.Client.Services.ContentService;

namespace MRA.Jobs.Client.Services.ConverterService;

public class DateTimeConverterToStringService(IContentService contentService)
    : IDateTimeConvertToStringService
{
    public string GetDisplayPostedDate(DateTime publishDate)
    {
        var currentDate = DateTime.Now;
        string displayDate;

        if (publishDate.Date == currentDate.Date)
        {
            if (publishDate.Minute == currentDate.Minute)
            {
                displayDate = $"{currentDate.Second - publishDate.Second} {contentService["ConverterService:SecondsAgo"]}";
            }
            else if (publishDate.Hour == currentDate.Hour)
            {
                displayDate = $"{currentDate.Minute - publishDate.Minute} {contentService["ConverterService:MinAgo"]}";
            }
            else if ((currentDate.Hour - publishDate.Hour) == 1)
            {
                displayDate = $"1 {contentService["ConverterService:HourAgo"]}";
            }
            else if ((currentDate.Hour - publishDate.Hour) <= 5)
            {
                displayDate = $"{currentDate.Hour - publishDate.Hour} {contentService["ConverterService:HoursAgo"]}";
            }
            else
            {
                displayDate = $"{contentService["ConverterService:TodayAt"]} {publishDate:HH:mm}";
            }
        }
        else if (publishDate.Date == currentDate.AddDays(-1).Date)
        {
            displayDate = contentService["ConverterService:Yesterday"];
        }
        else
        {
            displayDate = $"{publishDate:dd.MM.yyyy}";
        }

        return displayDate;
    }

    public (string DisplayDate, string Color) GetDeadlineOrEndDateDisplayDate(DateTime value)
    {
        var currentDate = DateTime.Now;
        string displayDate;
        string color = "gray";

        if (value.Date == currentDate.Date)
        {
            if (value.Minute == currentDate.Minute)
            {
                displayDate = $"{value.Second - currentDate.Second} {contentService["ConverterService:SecondsLeft"]}";
                color = "orange";
            }
            else if (value.Hour == currentDate.Hour)
            {
                displayDate = $"{value.Minute - currentDate.Minute} {contentService["ConverterService:MinLeft"]}";
                color = "orange";
            }
            else if ((value.Hour - currentDate.Hour) == 1)
            {
                displayDate = $"1 {contentService["ConverterService:HourLeft"]}";
                color = "orange";
            }
            else if ((value.Hour - currentDate.Hour) <= 5)
            {
                displayDate = $"{value.Hour - currentDate.Hour} {contentService["ConverterService:HoursLeft"]}";
                color = "orange";
            }
            else
            {
                displayDate = $"{contentService["ConverterService:TodayAt"]} {value:HH:mm}";
            }
        }
        else if (value.Date == currentDate.AddDays(1).Date)
        {
            displayDate = contentService["ConverterService:Tomorrow"];
        }
        else
        {
            displayDate = $"{contentService["ConverterService:On"]} {value:dd.MM.yyyy}";
        }

        if (value <= currentDate)
        {
            color = "red";
        }

        return (displayDate, color);
    }


}
