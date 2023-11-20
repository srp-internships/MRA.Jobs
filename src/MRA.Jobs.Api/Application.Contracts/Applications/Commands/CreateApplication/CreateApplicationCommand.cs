using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommand : IRequest<Guid>
{
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public IEnumerable<VacancyResponseDto> VacancyResponses { get; set; }

    public Cv Cv { get; set; } = new();
}

#nullable enable
public class Cv
{
    public bool IsUploadCvMode { get; set; }
    public byte[]? CvBytes { get; set; }
    /// <summary>
    /// fileName may not be unique.
    /// server should make it unique.
    /// </summary>
    public string? FileName { get; set; }
}