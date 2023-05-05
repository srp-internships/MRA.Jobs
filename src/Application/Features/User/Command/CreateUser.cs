using FluentValidation;
using MediatR;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Contracts.User.Command;

namespace MRA.Jobs.Application.Features.User.Command;

using Domain.Entities;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User()
        {
            Avatar = request.Avatar,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            DateOfBrith = request.DateOfBrith
        };

        _context.DomainUsers.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(a => a.Avatar)
            .NotEmpty()
            .MaximumLength(60);

        RuleFor(f => f.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);
        
        RuleFor(l => l.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(d => d.DateOfBrith)
            .NotEmpty();
        
        RuleFor(p => p.PhoneNumber)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(c => c.CreatedAt)
            .NotEmpty();
    }
}