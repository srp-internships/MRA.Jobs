using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.AplicationNotes.Commands;
public class CreatAplicationNoteCommand:IRequest<long>
{
    public long AplicationId { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public long? UserId { get; set; }
}
