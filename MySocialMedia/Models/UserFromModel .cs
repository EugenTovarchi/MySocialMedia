﻿using MySocialMedia.Models.Users;
using MySocialMedia.ViewModels.Account;

namespace MySocialMedia.Models;

public static class UserFromModel
{
    public static User Convert(this User user, UserEditViewModel usereditvm)
    {
        user.Image = usereditvm.Image;  
        user.LastName = usereditvm.LastName;
        user.MiddleName = usereditvm.MiddleName;
        user.FirstName = usereditvm.FirstName;
        user.Email = usereditvm.Email;
        user.BirthDate = usereditvm.BirthDate;
        user.UserName = usereditvm.UserName; //мыло - котороя явл логином(при смене должно меняться)
        user.Status = usereditvm.Status;
        user.About = usereditvm.About;

        return user;
    }
}
