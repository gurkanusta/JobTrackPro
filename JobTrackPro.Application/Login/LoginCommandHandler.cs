

using JobTrackPro.Application.Auth.Commands.Register;
using JobTrackPro.Application.Common.Interfaces;

using MediatR;

namespace JobTrackPro.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return new AuthResult(result.IsSuccess, result.Token, result.RefreshToken, result.Error);
    }
}