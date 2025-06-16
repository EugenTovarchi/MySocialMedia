namespace MySocialMedia.Views.ViewModels;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MySocialMedia.Models;
using MySocialMedia.Models.Repositories;

public class UserViewModel
{
    public User User { get; set; }

    public UserViewModel(User user)
    {
        User = user;
    }
}
