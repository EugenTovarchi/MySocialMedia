namespace MySocialMedia.Views.ViewModels.Account;

public class UserEditViewModel
{
    public string UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public DateTime BirthDate { get; set; }

    public string? Email { get; set; }
    public string UserName => Email;  // логин по почте

    public string Image { get; set; }

    public string Status { get; set; }

    public string About { get; set; }
}
