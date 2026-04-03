

using JobTrackPro.Application.Common.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace JobTrackPro.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<AuthServiceResult> RegisterAsync(
        string firstName, string lastName, string email, string password,
        CancellationToken cancellationToken = default)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
            return new AuthServiceResult(false, null, "This email is already registered.");

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
            return new AuthServiceResult(false, null, errors);
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email!, user.FirstName, user.LastName);
        return new AuthServiceResult(true, token, null);
    }

    public async Task<AuthServiceResult> LoginAsync(
        string email, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new AuthServiceResult(false, null, "Invalid email or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return new AuthServiceResult(false, null, "Invalid email or password.");

        var token = _jwtService.GenerateToken(user.Id, user.Email!, user.FirstName, user.LastName);
        return new AuthServiceResult(true, token, null);
    }
}