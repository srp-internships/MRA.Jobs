using System;
using System.ComponentModel.DataAnnotations;

namespace MRA.Identity.Domain.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; }

    public Guid UserId { get; set; }

    public DateTime ExpiryOn { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedByIp { get; set; }

    public string RevokedByIp { get; set; }

    public DateTime RevokedOn { get; set; }
}