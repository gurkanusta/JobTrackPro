using FluentAssertions;

using JobTrackPro.Application.Auth.Commands.Register;
using JobTrackPro.Application.Common.Interfaces;

using Moq;

namespace JobTrackPro.Tests.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IAuthService> _authMock = new();

    [Fact]
    public async Task Handle_NewUser_ReturnsSuccessWithTokens()
    {
       
        _authMock.Setup(x => x.RegisterAsync("gurkan", "usta", "usta@test.com", "pass123!", default))
                 .ReturnsAsync(new AuthServiceResult(true, "jwt-token", "refresh-token", null));

        var handler = new RegisterCommandHandler(_authMock.Object);
        var command = new RegisterCommand("gurkan", "usta", "usta@test.com", "pass123!");
        
        var result = await handler.Handle(command, default);

        
        result.IsSuccess.Should().BeTrue();
        result.Token.Should().Be("jwt-token");
        result.RefreshToken.Should().Be("refresh-token");
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsFailure()
    {
  
        _authMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), "existing@test.com", It.IsAny<string>(), default))
                 .ReturnsAsync(new AuthServiceResult(false, null, null, "This email is already registered."));

        var handler = new RegisterCommandHandler(_authMock.Object);
        var command = new RegisterCommand("gurkan", "usta", "existing@test.com", "pass123!");

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("This email is already registered.");
    }
}