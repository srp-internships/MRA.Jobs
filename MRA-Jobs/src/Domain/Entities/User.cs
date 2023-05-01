namespace MRA_Jobs.Domain.Entities;
public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime BirthDay { get; set; }
    public long RoleId { get; set; }
    public ICollection<Note> Notes { get; set; }
}
