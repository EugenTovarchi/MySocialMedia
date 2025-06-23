using MySocialMedia.Models.Users;

namespace MySocialMedia.Extensions;

/// <summary>
/// для вьюмодели  будем ставить признак, доступно ли добавление в друзья.
/// </summary>
public class UserWithFriendExt : User
{
    public bool IsFriendWithCurrent { get; set; }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Image { get; set; } // Должно быть
    public string About { get; set; } // Должно быть
    public string Status { get; set; } // Должно быть

    public string GetFullName() => $"{FirstName} {LastName}";
}
