using System.ComponentModel.DataAnnotations;

namespace MySocialMedia.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    [Display(Name = "Email", Prompt = "Write your email: ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
    [DataType(DataType.Password)]       //тип сделали для того чтобы скрывать пароль при вводе
    [Display(Name = "Пароль", Prompt = "Write password: ")]
    [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
    public string Password { get; set; }

    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }
}
