using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocialMedia.Models.Users;
using MySocialMedia.ViewModels.Account;

namespace MySocialMedia.Controllers.Account;

public class RegisterController : Controller
{
    private IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<RegisterController> _logger;
    public RegisterController(IMapper mapper, UserManager<User> userManager,
        SignInManager<User> signInManager, ILogger<RegisterController> logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterViewModel();

#if DEBUG
        // Тестовые данные только в режиме отладки
        model.FirstName = "Тестовый";
        model.LastName = "Пользователь";
        model.BirthDate = DateTime.Now.AddYears(-20); // 20 лет назад
#endif
        return View("~/Views/Shared/Register.cshtml");
    }

    [Route("RegisterPart2")]
    [HttpGet]  
    public IActionResult RegisterPart2(RegisterViewModel model)
    {
        return View("RegisterPart2", model); // Показ второй стадии
    }

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
#if DEBUG
        // Логирование тестовых данных
        _logger.LogDebug($"Регистрация в DEBUG режиме. Данные: {model.FirstName} {model.LastName}, {model.BirthDate}");
#endif
        var user = new User
        {
            UserName = model.EmailReg,  // Используем EmailReg как UserName
            Email = model.EmailReg,
            FirstName = model.FirstName,
            LastName = model.LastName,
            BirthDate = model.BirthDate, //вот тут была проблема, что дата не передавалась во 2 часть регистрации
        };

        var result = await _userManager.CreateAsync(user, model.PasswordReg);

        if (result.Succeeded)
        {  
            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation($"Пользователь {user.UserName} успешно прошёл регистрацию.");
            _logger.LogInformation($"Received BirthDate: {model.BirthDate}");

            return RedirectToAction("MyPage", "AccountManager");
        }

        // Если дошли сюда, значит были ошибки
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
            _logger.LogError(error.Description);
        }
        // Возвращаем на вторую страницу с ошибками
        return View("~/Views/Shared/RegisterPart2.cshtml", model);
    }
}
