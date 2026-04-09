
using Microsoft.AspNetCore.Identity;

namespace JobTrackPro.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}