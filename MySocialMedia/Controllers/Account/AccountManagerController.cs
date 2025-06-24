using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocialMedia.Extensions;
using MySocialMedia.Models;
using MySocialMedia.Models.Repositories;
using MySocialMedia.Models.UoW;
using MySocialMedia.Models.Users;
using MySocialMedia.ViewModels.Account;

namespace MySocialMedia.Controllers.Account;

[Route("AccountManager")]
public class AccountManagerController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountManagerController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager,
        IMapper mapper, ILogger<AccountManagerController> logger, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    [Route("Generate")]
    [HttpGet]
    public async Task<IActionResult> Generate()
    {

        var usergen = new GenetateUsers();
        var userlist = usergen.Populate(35);

        foreach (var user in userlist)
        {
            var result = await _userManager.CreateAsync(user, "123456");

            if (!result.Succeeded)
                continue;
        }

        return RedirectToAction("Index", "Home");
    }

    [Route("Login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View("Home/Login");
    }

    [HttpPost]
    [Route("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)   
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверные учетные данные");
                return View("~/Views/Home/Login.cshtml", model);
            }

            var result = await _signInManager.PasswordSignInAsync(   
                user.UserName, 
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Успешный вход. UserId: {user.Id}");
                return RedirectToAction("MyPage", "AccountManager");
            }

            ModelState.AddModelError(string.Empty, "Неверные учетные данные");
        }

        return View("~/Views/Home/Login.cshtml", model);
    }

    [Route("Logout")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Route("Edit")]
    [HttpGet]
    public IActionResult Edit()
    {
        var user = User;

        var result = _userManager.GetUserAsync(user);

        var editmodel = _mapper.Map<UserEditViewModel>(result.Result);

        return View("Edit", editmodel);
    }

    [Route("Update")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Update(UserEditViewModel model)
    {
        _logger.LogInformation("Запрос получен. ModelState: {@ModelState}", ModelState);
        _logger.LogInformation("Данные модели: {@Model}", model);
        _logger.LogInformation("Начало обработки Update для пользователя {UserId}", model.UserId);

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            _logger.LogInformation("Входим в user.Convert()");
            user.Convert(model);

            _logger.LogInformation("Вышли из Конверта и применяем изменения в result");
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
        return View("UserList", model); //
    }

   

    [Route("AddFriend")]
    [HttpPost]
    public async Task<IActionResult> AddFriend(string id)
    {
        var currentuser = User;  // Текущий авторизованный пользователь (из контекста HTTP)

        var result = await _userManager.GetUserAsync(currentuser);  // Получаем полные данные пользователя из БД

        var friend = await _userManager.FindByIdAsync(id);

        var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;  //преобразовует к FriendRepo т.к. метод в нем, а не в IRepository

        await repository.AddFriend(result, friend);

        return RedirectToAction("MyPage", "AccountManager");

    }

    [Route("DeleteFriend")]
    [HttpPost]
    public async Task <IActionResult> DeleteFriend (string id)
    {
        var currentuser = User; 
        var result = await _userManager.GetUserAsync(currentuser);
        var friend = await _userManager.FindByIdAsync (id);

        var repos = _unitOfWork.GetRepository<Friend>() as FriendsRepository;
        await repos.DeleteFriend(result, friend);

        return RedirectToAction("MyPage", "AccountManager");
    }

    [Authorize]
    [Route("MyPage")]
    [HttpGet]
    public async Task<IActionResult> MyPage()  
    {
        try
        {
            var user = await _userManager.GetUserAsync(User); 
            if (user == null)
            {
                _logger.LogWarning("Поользователь не найдет после аутентификации");
                return RedirectToAction("Login");
            }

            _logger.LogInformation($"Загружаем страницу MyPage для : {user.Email}");

            var model = new UserViewModel(user)
            {
                Friends = await GetAllFriend(user)
            };

            return View("User", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка в  MyPage");
            return RedirectToAction("Error", "Home");
        }
    }

    [Route("Chat")]
    [HttpPost]
    public async Task<IActionResult> Chat(string id)
    {
        var currentuser = User;

        var result = await _userManager.GetUserAsync(currentuser);
        var friend = await _userManager.FindByIdAsync(id);

        var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

        var mess = await repository.GetMessages(result, friend);

        var model = new ChatViewModel()
        {
            You = result,
            ToWhom = friend,
            History = mess.OrderBy(x => x.Id).ToList(),
        };
        return View("Chat", model);
    }

    [Route("NewMessage")]
    [HttpPost]
    public async Task<IActionResult> NewMessage(string id, ChatViewModel chat)
    {
        var currentuser = User;

        var result = await _userManager.GetUserAsync(currentuser);
        var friend = await _userManager.FindByIdAsync(id);

        var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

        var item = new Message()
        {
            Sender = result,
            Recipient = friend,
            Text = chat.NewMessage.Text,
        };
        await repository.Create(item);
        await _unitOfWork.SaveChanges();

        var mess = await repository.GetMessages(result, friend);

        var model = new ChatViewModel()
        {
            You = result,
            ToWhom = friend,
            History = mess.OrderBy(x => x.Id).ToList(), 
        };
        return View("Chat", model);
    }

    private async Task<ChatViewModel> GenerateChat(string id)
    {
        var currentuser = User;

        var result = await _userManager.GetUserAsync(currentuser);
        var friend = await _userManager.FindByIdAsync(id);

        var repository =  _unitOfWork.GetRepository<Message>() as MessageRepository;

        var message = await repository.GetMessages(result, friend);

        var model = new ChatViewModel()
        {
            You = result,
            ToWhom = friend,
            History = message.OrderBy(x => x.Id).ToList(),
        };

        return model;
    }

    [Route("Chat")]
    [HttpGet]
    public async Task<IActionResult> Chat()
    {

        var id = Request.Query["id"];

        var model = await GenerateChat(id);
        return View("Chat", model);
    }

    [Route("")]
    [Route("[controller]/[action]")]
    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            return RedirectToAction("MyPage", "AccountManager");
        }
        else
        {
            return View(new MainViewModel());
        }
    }

    private async Task<SearchViewModel> CreateSearch(string search)
    {
        var currentuser = User;
        var result = await _userManager.GetUserAsync(currentuser);


        var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
        if (!string.IsNullOrEmpty(search))
        {
            list = list.Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
        }

        var withfriend = await GetAllFriend();

        var data = new List<UserWithFriendExt>();   //Проверяется, является ли пользователь другом (есть ли он в списке друзей)
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

        return await repository.GetFriendsByUser(result);
    }

    private async Task<List<User>> GetAllFriend(User user)
    {
        var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

        return await repository.GetFriendsByUser(user);
    }

}
