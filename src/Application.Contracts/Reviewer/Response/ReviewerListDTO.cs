namespace MRA.Jobs.Application.Contracts.Reviewer.Response;

public class ReviewerListDTO
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string JobTitle { get; set; }
}

public class ReviewerDetailsDTO
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string JobTitle { get; set; }
}