using MRA.Identity.Domain.Common;

namespace MRA.Identity.Domain.Entities;

public class User:BaseEntity
{
    public string Email { get; set; } = "";
    public string Username { get; set; }="";
    public string Role { get; set; }="";
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

}