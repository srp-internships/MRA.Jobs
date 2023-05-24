using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;

namespace MRA.Jobs.Application.UnitTests.Applicant;

public class CreateApplicantCommandHandlerTests : BaseTestFixture
{
    private CreateApplicantCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateApplicantCommandHandler(
            _dbContextMock.Object,
            Mapper
        );
    }
    
    [Test]
    public async Task Handle_ValidRequest_ShouldCreateApplicant()
    {
        // Arrange
        var request = new CreateApplicantCommand
        {
            Avatar = "user_avatar",
            LastName = "userLastName",
            FirstName = "userFirstName",
            Email = "user@gmail.com",
            Patronymic = "user",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = "123456789"
        };

        var applicantSetMock = new Mock<DbSet<Domain.Entities.Applicant>>();
        var newEntityGiud = Guid.NewGuid();

        applicantSetMock.Setup(a => a.AddAsync(
            It.IsAny<Domain.Entities.Applicant>(),
            It.IsAny<CancellationToken>())
        ).Callback<Domain.Entities.Applicant, CancellationToken>((a, ct) => a.Id = newEntityGiud);
        _dbContextMock.Setup(d => d.Applicants).Returns(applicantSetMock.Object);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        // Assert
        result.Should().Be(newEntityGiud);
        
        applicantSetMock
            .Verify(x => x.AddAsync(
                It.Is<Domain.Entities.Applicant>(a => 
                        a.Avatar == request.Avatar && 
                        a.FirstName == request.FirstName &&
                        a.LastName == request.LastName &&
                        a.Email == request.Email &&
                        a.DateOfBrith == request.DateOfBirth && 
                        a.PhoneNumber == request.PhoneNumber
                ), It.IsAny<CancellationToken>()), Times.Once);
        
        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}