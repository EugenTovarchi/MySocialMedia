namespace MySocialMedia.ViewModels.Account;

using MySocialMedia.Extensions;
using MySocialMedia.Models.Users;

public class UserViewModel
{
    public User User { get; set; }

    public UserViewModel(User user)
    {
        User = user;
    }
   
    public List<User> Friends { get; set; }
}
