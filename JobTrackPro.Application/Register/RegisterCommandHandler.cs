

using JobTrackPro.Application.Common.Interfaces;

using MediatR;

namespace JobTrackPro.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            cancellationToken);

        return new AuthResult(result.IsSuccess, result.Token, result.Error);
    }
}