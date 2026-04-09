using JobTrackPro.Application.Common.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace JobTrackPro.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;

    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<AuthServiceResult> RegisterAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken = default)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
            return new AuthServiceResult(false, null, null, "This email is already registered.");

        var user = new AppUser
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim().ToLower(),
            UserName = email.Trim().ToLower(),
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthServiceResult(false, null, null, errors);
        }

        return await IssueTokensAsync(user);
    }

    public async Task<AuthServiceResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new AuthServiceResult(false, null, null, "Invalid email or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return new AuthServiceResult(false, null, null, "Invalid email or password.");

        return await IssueTokensAsync(user);
    }

    public async Task<AuthServiceResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

        if (user is null)
            return new AuthServiceResult(false, null, null, "Invalid refresh token.");

        if (user.RefreshTokenExpiry is null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return new AuthServiceResult(false, null, null, "Refresh token has expired. Please log in again.");

        return await IssueTokensAsync(user);
    }

    public async Task RevokeTokenAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await _userManager.UpdateAsync(user);
    }

    private async Task<AuthServiceResult> IssueTokensAsync(AppUser user)
    {
        var accessToken = _jwtService.GenerateToken(user.Id, user.Email!, user.FirstName, user.LastName);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.Add(RefreshTokenLifetime);
        await _userManager.UpdateAsync(user);

        return new AuthServiceResult(true, accessToken, refreshToken, null);
    }
}