using FluentAssertions;

using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.Commands.CreateJob;
using JobTrackPro.Domain.Entities;
using JobTrackPro.Domain.Enums;

using Moq;

namespace JobTrackPro.Tests.Jobs;

public class CreateJobCommandHandlerTests
{
    private readonly Mock<IAppDbContext> _dbMock = new();
    private readonly Mock<ICurrentUserService> _userMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsGuid()
    {
  
        _userMock.Setup(x => x.UserId).Returns("user-123");
        _dbMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>()));
        _dbMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        var handler = new CreateJobCommandHandler(_dbMock.Object, _userMock.Object);
        var command = new CreateJobCommand(
            "Google", "Backend Developer", ApplicationStatus.Applied,
            null, null, null, null, null);

        var result = await handler.Handle(command, default);

        result.Should().NotBeEmpty();
        _dbMock.Verify(x => x.AddJobApplication(It.IsAny<JobApplication>()), Times.Once);
        _dbMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotAuthenticated_ThrowsUnauthorizedException()
    {
        
        _userMock.Setup(x => x.UserId).Returns((string?)null);

        var handler = new CreateJobCommandHandler(_dbMock.Object, _userMock.Object);
        var command = new CreateJobCommand(
            "Google", "Backend Developer", ApplicationStatus.Applied,
            null, null, null, null, null);

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ValidCommand_TrimsCompanyName()
    {
        
        _userMock.Setup(x => x.UserId).Returns("user-123");
        JobApplication? saved = null;
        _dbMock.Setup(x => x.AddJobApplication(It.IsAny<JobApplication>()))
               .Callback<JobApplication>(j => saved = j);
        _dbMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        var handler = new CreateJobCommandHandler(_dbMock.Object, _userMock.Object);
        var command = new CreateJobCommand(
            "  Google  ", "  Backend  ", ApplicationStatus.Applied,
            null, null, null, null, null);

        await handler.Handle(command, default);

        saved!.CompanyName.Should().Be("Google");
        saved.Position.Should().Be("Backend");
    }
}