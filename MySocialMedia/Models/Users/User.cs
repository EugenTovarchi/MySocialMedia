using Microsoft.AspNetCore.Identity;

namespace MySocialMedia.Models.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public string? MiddleName { get; set; }

    public DateTime BirthDate { get; set; }

    public string Image { get; set; }

    public string Status { get; set; }

    public string About { get; set; }

    public string GetFullName()
    {
        return FirstName + " " + MiddleName + " " + LastName;
    }

    public User()
    {
        Image = "https://sun9-80.userapi.com/s/v1/if2/bH_WV1ge6qHHbWsuvA8CwtOAmfiJWO-rDzPVO1ro0xsLumQI_VQKqphx8fBg4gOh5A7LxMEsY8EzHn03HQOCPj4Y.jpg?quality=95&as=32x22,48x33,72x49,108x73,160x109,240x163,360x244,480x326,540x366,640x434,720x489,1080x733&from=bu&u=uGoOgKQJPzmlZUf0TU6ql1ScARDV6v2XL_b5Wb-8Eig&cs=640x0";  //https://thispersondoesnotexist.com
        Status = "Ура! Я в соцсети!";
        About = "Информация обо мне.";
    }
}
