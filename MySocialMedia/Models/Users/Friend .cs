using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialMedia.Models.Users;

[Table("UserFriends")]
public class Friend
{
    public int Id { get; set; }  //PK
    public string UserId { get; set; }  //ID пользователя, который добавляет в друзья (AspNetUsers.Id)
    public string CurrentFriendId { get; set; }  //ID пользователя, которого добавляют в друзья (AspNetUsers.Id)

    public User User { get; set; }   //Навигационное свойство (ссылка на объект пользователя)

    public User CurrentFriend { get; set; }  //	Навигационное свойство (ссылка на объект друга)
}
