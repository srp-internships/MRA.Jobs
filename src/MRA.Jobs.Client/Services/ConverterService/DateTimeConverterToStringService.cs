namespace MRA.Jobs.Client.Services.ConverterService;

public class DateTimeConverterToStringService : IDateTimeConvertToStringService
{
    public  string GetDisplayPostedDate(DateTime publishDate)
    {
        var currentDate = DateTime.Now;
        string displayDate;

        if (publishDate.Date == currentDate.Date)
        {
            if (publishDate.Minute == currentDate.Minute)
            {
                displayDate = $"{currentDate.Second - publishDate.Second} seconds ago";
            }
            else if (publishDate.Hour == currentDate.Hour)
            {
                displayDate = $"{currentDate.Minute - publishDate.Minute} min ago";
            }
            else if ((currentDate.Hour - publishDate.Hour) == 1)
            {
                displayDate = $"1 hour ago";
            }
            else if ((currentDate.Hour - publishDate.Hour) <= 5)
            {
                displayDate = $"{currentDate.Hour - publishDate.Hour} hours ago";
            }
            else
            {
                displayDate = $"Today at {publishDate:HH:mm}";
            }
        }
        else if (publishDate.Date == currentDate.AddDays(-1).Date)
        {
            displayDate = $"Yesterday";
        }
        else
        {
            displayDate = $"{publishDate:dd.MM.yyyy}";
        }

        return displayDate;
    }

    public  (string DisplayDate, string Color) GetDeadlineOrEndDateDisplayDate(DateTime value)
    {
        var currentDate = DateTime.Now;
        string displayDate;
        string color = "gray";

        if (value.Date == currentDate.Date)
        {
            if (value.Minute == currentDate.Minute)
            {
                displayDate = $"{value.Second - currentDate.Second} seconds left";
                color = "orange";
            }
            else if (value.Hour == currentDate.Hour)
            {
                displayDate = $"{value.Minute - currentDate.Minute} min left";
                color = "orange";
            }
            else if ((value.Hour - currentDate.Hour) == 1)
            {
                displayDate = $"1 hour left";
                color = "orange";
            }
            else if ((value.Hour - currentDate.Hour) <= 5)
            {
                displayDate = $"{value.Hour - currentDate.Hour} hours left";
                color = "orange";
            }
            else
            {
                displayDate = $"Today at {value:HH:mm}";
            }
        }
        else if (value.Date == currentDate.AddDays(1).Date)
        {
            displayDate = $"Tomorrow";
        }
        else
        {
            displayDate = $"on {value:dd.MM.yyyy}";
        }

        if (value <= currentDate)
        {
            color = "red";
        }

        return (displayDate, color);
    }


}
