using Microsoft.AspNetCore.Identity;

namespace Hostele.Models;

public class AppUser:IdentityUser
{
    public string Name { get; set; }
    public int UsernameChangeLimit { get; set; } = 15;
    public byte[]? ProfilePicture { get; set; }
}