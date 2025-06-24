using AspNetCoreGeneratedDocument;
using MySocialMedia.Extensions;
using MySocialMedia.Models.Users;

namespace MySocialMedia.ViewModels.Account;

public class SearchViewModel
{
    public List<UserWithFriendExt> UserList { get; set; }

    public SearchViewModel()
    {
        UserList = new List<UserWithFriendExt>();
    }

    public SearchViewModel(IEnumerable<User> users, User currentUser, IEnumerable<User> friends)
    {
        UserList = users.Select(u => new UserWithFriendExt
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Image = u.Image ?? "/images/default-avatar.png",
            About = u.About,
            Status = u.Status,
            IsFriendWithCurrent = friends.Any(f => f.Id == u.Id) || u.Id == currentUser.Id
        }).ToList();
    }

}


