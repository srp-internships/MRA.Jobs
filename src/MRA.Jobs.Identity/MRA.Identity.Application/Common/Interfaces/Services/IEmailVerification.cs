﻿using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IEmailVerification
{
    Task<bool> SendVerificationEmailAsync(ApplicationUser user);
    Task VerifyEmailAsync(string token, Guid userId);
    Task<bool> EmailApproved(ApplicationUser user);
    Task<bool> EmailRejected(ApplicationUser user);
}