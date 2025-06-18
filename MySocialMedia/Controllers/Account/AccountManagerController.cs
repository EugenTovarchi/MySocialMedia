using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocialMedia.Extensions;
using MySocialMedia.Models;
using MySocialMedia.Models.Repositories;
using MySocialMedia.Models.Repositoriesж;
using MySocialMedia.Views.ViewModels;
using MySocialMedia.Views.ViewModels.Account;

namespace MySocialMedia.Controllers.Account;

public class AccountManagerController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountManagerController> _logger;

    public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager,
        IMapper mapper, ILogger<AccountManagerController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
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
    public async Task<IActionResult> Login(LoginViewModel model, ILogger<AccountManagerController> logger)
    {
        if (ModelState.IsValid)
        {

            var user = _mapper.Map<User>(model);

            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    _logger.LogInformation("Пользователь вошел в систему!Метод Login - Post");
                    return RedirectToAction("MyPage", "AccountManager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
        }
        return View("model"); //Views/Home/Index.cshtml
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

        _logger.LogInformation($"My User: {result} авторизован");
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
    [HttpGet]
    public async Task<IActionResult> UserList(string search)
    {
        var model = await CreateSearch(search);
        return View("UserList", model);
    }

    private async Task<SearchViewModel> CreateSearch(string search)
    {
        var currentuser = User;
        var result = await _userManager.GetUserAsync(currentuser);

        var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
        var withfriend = await GetAllFriend();

        var data = new List<UserWithFriendExt>();
        list.ForEach(x =>
        {
            var t = _mapper.Map<UserWithFriendExt>(x);
            t.IsFriendWithCurrent = withfriend.Where(y => y.Id == x.Id || x.Id == result.Id).Count() != 0;
            data.Add(t);
        });

        var model = new SearchViewModel()
        {
            UserList = data
        };
        return model;
    }

    private async Task<List<User>> GetAllFriend()
    {
        var user = User;

        var result = await _userManager.GetUserAsync(user);

        var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

        return repository.GetFriendsByUser(result);
    }

    [Route("AddFriend")]
    [HttpPost]
    public async Task<IActionResult> AddFriend(string id)
    {
        var currentuser = User;

        var result = await _userManager.GetUserAsync(currentuser);

        var friend = await _userManager.FindByIdAsync(id);

        var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

        repository.AddFriend(result, friend);

        return RedirectToAction("MyPage", "AccountManager");

    }
}
