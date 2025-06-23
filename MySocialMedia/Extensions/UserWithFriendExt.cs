using MySocialMedia.Models.Users;

namespace MySocialMedia.Extensions;

/// <summary>
/// для вьюмодели  будем ставить признак, доступно ли добавление в друзья.
/// </summary>
public class UserWithFriendExt : User
{
    public bool IsFriendWithCurrent { get; set; }

}
