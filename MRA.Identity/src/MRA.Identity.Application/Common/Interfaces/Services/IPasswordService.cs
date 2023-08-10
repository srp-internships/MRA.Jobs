namespace MRA.Identity.Application.Common.Interfaces.Services;

public interface IPasswordService
{
    internal void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    internal bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}