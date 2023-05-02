using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA_Jobs.Infrastructure.Persistence;

namespace MRA_Jobs.Infrastructure.Services;
internal class NoteService : EntityService<Note>, INoteService
{
    public NoteService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
