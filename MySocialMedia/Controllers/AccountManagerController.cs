using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocialMedia.Models;
using MySocialMedia.Models.Repositories;
using MySocialMedia.Views.ViewModels;
using MySocialMedia.Views.ViewModels.Account;

namespace MySocialMedia.Controllers;

public class AccountManagerController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper =   mapper;
    }

    [Route("Login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View("Home/Login");
    }

    /// <summary>
    /// показываем форму входа и не выполняем саму аутентификацию.
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [Route("Login")]  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {

            var user = _mapper.Map<User>(model);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
        }
        return View("Views/Home/Index.cshtml");
    }

    [Route("Logout")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]   //только авторизованные пользователи могут попасть на свою страницу.
    [Route("MyPage")]
    [HttpGet]
    public IActionResult MyPage()
    {
        // Получаем объект пользователя из контекста HTTP-запроса
        var user = User;

        // Асинхронно получаем данные пользователя из базы через UserManager
        var result = _userManager.GetUserAsync(user);

        // Создаём ViewModel и передаём её в представление "User"
        return View("User", new UserViewModel(result.Result));
    }

    [Authorize]
    [Route("Update")]
    [HttpPost]
    public async Task<IActionResult> Update(UserEditViewModel model)
    {
        if (ModelState.IsValid)  //Проверяет валидационные атрибуты модели
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            user.Convert(model);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("MyPage", "AccountManager");
            }
            else
            {
                return RedirectToAction("Edit", "AccountManager");
            }
        }
        else
        {
            ModelState.AddModelError("", "Некорректные данные");
            return View("Edit", model);
        }
    }

    /// <summary>
    /// Поиск пользователей
    /// </summary>
    /// <returns></returns>
    [Route("UserList")]
    [HttpPost]
    public IActionResult UserList()
    {
        var model = new SearchViewModel()
        {
            UserList = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().Contains(search)).ToList()
        };
        return View("UserList", model);
    }
}
