using Microsoft.AspNetCore.Identity;
using MySocialMedia.Extensions;
using MySocialMedia.Models;
using MySocialMedia.Models.Repositories;

namespace MySocialMedia.ViewModels.Account;

public class SearchViewModel
{
    // public List<User> UserList { get; set; }
    public List<UserWithFriendExt> UserList { get; set; }
}
