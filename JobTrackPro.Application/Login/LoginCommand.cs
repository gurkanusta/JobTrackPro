using JobTrackPro.Application.Auth.Commands.Register;

using MediatR;

namespace JobTrackPro.Application.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResult>;