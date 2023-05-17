using MRA.Jobs.Application.Contracts.Applicant.Commands;
using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Applicant.Command.CreateApplicant;
using MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;
using NUnit.Framework.Internal;

namespace MRA.Jobs.Application.UnitTests.Reviewer;
using Domain.Entities;

public class CreateReviewerCommandHandlerTests : BaseTestFixture
{
    private CreateReviewerCommandHandler _handler;

    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateReviewerCommandHandler(
            _dbContextMock.Object,
            Mapper
        );
    }
    
    [Test]
    public async Task Handle_ValidRequest_ShouldCreateReviewer()
    {
        // Arrange
        var request = new CreateReviewerCommand()
        {
            Avatar = "user_avatar",
            LastName = "userLastName",
            FirstName = "userFirstName",
            Email = "user@gmail.com",
            DateOfBrith = DateTime.UtcNow,
            PhoneNumber = "123456789",
        };

        var reviewerSetMock = new Mock<DbSet<Reviewer>>();
        var newEntityGiud = Guid.NewGuid();

        reviewerSetMock.Setup(a => a.AddAsync(
            It.IsAny<Reviewer>(),
            It.IsAny<CancellationToken>())
        ).Callback<Reviewer, CancellationToken>((a, ct) => a.Id = newEntityGiud);
        _dbContextMock.Setup(d => d.Reviewers).Returns(reviewerSetMock.Object);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        // Assert
        result.Should().Be(newEntityGiud);
        
        reviewerSetMock
            .Verify(x => x.AddAsync(
                It.Is<Reviewer>(a => 
                    a.Avatar == request.Avatar && 
                    a.FirstName == request.FirstName &&
                    a.LastName == request.LastName &&
                    a.Email == request.Email &&
                    a.DateOfBrith == request.DateOfBrith && 
                    a.PhoneNumber == request.PhoneNumber
                ), It.IsAny<CancellationToken>()), Times.Once);
        
        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

}