namespace MRA_Jobs.Domain.Entities;

public class Note 
{
    public int AplicationId  { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public  User User { get; set; }
}
