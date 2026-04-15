using FluentAssertions;

using JobTrackPro.Application.Common.Exceptions;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.Commands.UpdateJob;
using JobTrackPro.Domain.Entities;
using JobTrackPro.Domain.Enums;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace JobTrackPro.Tests.Jobs;

public class UpdateJobCommandHandlerTests
{
    private readonly Mock<IAppDbContext> _dbMock = new();

    [Fact]
    public async Task Handle_ExistingJob_UpdatesSuccessfully()
    {

        var jobId = Guid.NewGuid();
        var job = new JobApplication
        {
            Id = jobId,
            CompanyName = "OldCompany",
            Position = "OldPosition",
            Status = ApplicationStatus.Applied,
            UserId = "user-1"
        };

        _dbMock.Setup(x => x.JobApplications)
               .Returns(new[] { job }.AsQueryable());
        _dbMock.Setup(x => x.UpdateJobApplication(It.IsAny<JobApplication>()));
        _dbMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        var handler = new UpdateJobCommandHandler(_dbMock.Object);
        var command = new UpdateJobCommand(
            jobId, "NewCompany", "NewPosition",
            ApplicationStatus.Interview, DateTime.UtcNow,
            null, null, null, null);

        await handler.Handle(command, default);

        job.CompanyName.Should().Be("NewCompany");
        job.Position.Should().Be("NewPosition");
        job.Status.Should().Be(ApplicationStatus.Interview);
        _dbMock.Verify(x => x.UpdateJobApplication(job), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingJob_ThrowsNotFoundException()
    {
        
        _dbMock.Setup(x => x.JobApplications)
               .Returns(Array.Empty<JobApplication>().AsQueryable());

        var handler = new UpdateJobCommandHandler(_dbMock.Object);
        var command = new UpdateJobCommand(
            Guid.NewGuid(), "Company", "Position",
            ApplicationStatus.Applied, DateTime.UtcNow,
            null, null, null, null);

        var act = async () => await handler.Handle(command, default);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}