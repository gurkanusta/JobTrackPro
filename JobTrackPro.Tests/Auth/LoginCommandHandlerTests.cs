using FluentAssertions;
using JobTrackPro.Application.Auth.Commands.Register;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Auth.Commands.Login;
using Moq;

namespace JobTrackPro.Tests.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthService> _authMock = new();

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccessWithToken()
    {
        // Arrange
        _authMock.Setup(x => x.LoginAsync("test@test.com", "pass123", default))
                 .ReturnsAsync(new AuthServiceResult(true, "jwt-token", "refresh-token", null));

        var handler = new LoginCommandHandler(_authMock.Object);
        var command = new LoginCommand("test@test.com", "pass123");

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        result.Token.Should().Be("jwt-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsFailure()
    {
        
        _authMock.Setup(x => x.LoginAsync("test@test.com", "wrong", default))
                 .ReturnsAsync(new AuthServiceResult(false, null, null, "Invalid email or password."));

        var handler = new LoginCommandHandler(_authMock.Object);
        var command = new LoginCommand("test@test.com", "wrong");

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
        result.Token.Should().BeNull();
        result.Error.Should().Be("Invalid email or password.");
    }
}