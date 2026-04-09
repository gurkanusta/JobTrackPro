

using MediatR;

namespace JobTrackPro.Application.Auth.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<AuthResult>;


public record AuthResult(
    bool IsSuccess,
    string? Token,
    string? RefreshToken,
    string? Error
);