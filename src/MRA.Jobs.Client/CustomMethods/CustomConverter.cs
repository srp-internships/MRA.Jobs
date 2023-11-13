namespace MRA.Jobs.Client.CustomMethods;

public static class CustomConverter
{
    public static string GetDisplayPostedDate(DateTime publishDate)
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
            displayDate = $"Yesterday at {publishDate:HH:mm}";
        }
        else
        {
            displayDate = $"{publishDate:dd.MM.yyyy}";
        }

        return displayDate;
    }


    public static string GetDeadlineDisplayDate(DateTime deadlineDate)
    {
        var currentDate = DateTime.Now;
        string displayDate;

        if (deadlineDate.Date == currentDate.Date)
        {
            if (deadlineDate.Minute == currentDate.Minute)
            {
                displayDate = $"{deadlineDate.Second - currentDate.Second} seconds left";
            }
            else if (deadlineDate.Hour == currentDate.Hour)
            {
                displayDate = $"{deadlineDate.Minute - currentDate.Minute} min left";
            }
            else if ((deadlineDate.Hour - currentDate.Hour) == 1)
            {
                displayDate = $"1 hour left";
            }
            else if ((deadlineDate.Hour - currentDate.Hour) <= 5)
            {
                displayDate = $"{deadlineDate.Hour - currentDate.Hour} hours left";
            }
            else
            {
                displayDate = $"Today at {deadlineDate:HH:mm}";
            }
        }
        else if (deadlineDate.Date == currentDate.AddDays(1).Date)
        {
            displayDate = $"Tomorrow at {deadlineDate:HH:mm}";
        }
        else
        {
            displayDate = $"on {deadlineDate:dd.MM.yyyy}";
        }

        return $"Deadline {displayDate}";
    }

}
