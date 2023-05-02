using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA_Jobs.Application.Common.Models.Dtos.NoteDtos;
public class GetNoteDto
{
    public long Id { get; set; }
    public long AplicationId { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public User? User { get; set; }
    public long UserId { get; set; }
}
