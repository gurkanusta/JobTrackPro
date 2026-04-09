namespace JobTrackPro.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthServiceResult> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<AuthServiceResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<AuthServiceResult> RefreshTokenAsync(
        
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task RevokeTokenAsync(string userId, CancellationToken cancellationToken = default);
}


public record AuthServiceResult(
    bool IsSuccess,
    string? Token,
    string? RefreshToken,
    string? Error
);