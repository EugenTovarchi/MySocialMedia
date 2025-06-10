using Microsoft.AspNetCore.Identity;

namespace MySocialMedia.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public DateTime BirthDate { get; set; }
}
