using System.ComponentModel.DataAnnotations;

namespace MySocialMedia.ViewModels.Account;

public class RegisterViewModel
{

    [Required]
    [Display(Name = "Имя")]
    public string?   FirstName { get; set; }

    [Required]
    [Display(Name = "Фамилия")]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    [Display(Name = "Email (он же ваш логин в дальнейшем)")]
    public string? EmailReg { get; set; }

    // Делаем Login необязательным в форме (скрытым или вычисляемым)
    [Display(Name = "Логин (повторите ваш email)")]
    public string? Login { get; set; }   // Автоматически берётся из EmailReg

    [DataType(DataType.Date)]  //указывает что свойство содержит дату без времени
    [Display(Name = "Дата рождения")]
    [Required(ErrorMessage = "Пожалуйста, укажите дату рождения")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  // задает формат даты и применять формат при редактировании
    public DateTime BirthDate { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
    public string? PasswordReg { get; set; }

    [Required]
    [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string? PasswordConfirm { get; set; }
}
