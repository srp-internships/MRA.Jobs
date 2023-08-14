﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Reviewer.Command;
public class AddTagsToReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public string[] Tags { get; set; }
}
