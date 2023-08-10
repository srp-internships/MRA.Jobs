using MRA.Jobs.Application.Contracts.Reviewer.Command;
using MRA.Jobs.Application.Features.Reviewer.Command.CreateReviewer;

namespace MRA.Jobs.Application.UnitTests.Reviewer;

public class CreateReviewerCommandHandlerTests : BaseTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();

        _handler = new CreateReviewerCommandHandler(
            _dbContextMock.Object,
            Mapper
        );
    }

    private CreateReviewerCommandHandler _handler;

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateReviewer()
    {
        // Arrange
        CreateReviewerCommand request = new CreateReviewerCommand
        {
            Avatar = "user_avatar",
            LastName = "userLastName",
            FirstName = "userFirstName",
            Email = "user@gmail.com",
            DateOfBirth = DateTime.UtcNow,
            PhoneNumber = "123456789"
        };

        Mock<DbSet<Domain.Entities.Reviewer>> reviewerSetMock = new Mock<DbSet<Domain.Entities.Reviewer>>();
        Guid newEntityGiud = Guid.NewGuid();

        reviewerSetMock.Setup(a => a.AddAsync(
            It.IsAny<Domain.Entities.Reviewer>(),
            It.IsAny<CancellationToken>())
        ).Callback<Domain.Entities.Reviewer, CancellationToken>((a, ct) => a.Id = newEntityGiud);
        _dbContextMock.Setup(d => d.Reviewers).Returns(reviewerSetMock.Object);

        // Act
        Guid result = await _handler.Handle(request, CancellationToken.None);
        // Assert
        result.Should().Be(newEntityGiud);

        reviewerSetMock
            .Verify(x => x.AddAsync(
                It.Is<Domain.Entities.Reviewer>(a =>
                    a.Avatar == request.Avatar &&
                    a.FirstName == request.FirstName &&
                    a.LastName == request.LastName &&
                    a.Email == request.Email &&
                    a.DateOfBirth == request.DateOfBirth &&
                    a.PhoneNumber == request.PhoneNumber
                ), It.IsAny<CancellationToken>()), Times.Once);

        _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}