using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySocialMedia.Models;
using MySocialMedia.Views.ViewModels;

namespace MySocialMedia.Controllers;

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

    [Route("Register")]
    [HttpGet]
    public IActionResult Register()
    {
        _logger.LogInformation($"Зашли в IActionResult Register1 ");   // используем всегда логер вместо CW ! 
        return View("Shared/Register");
    }

    [Route("RegisterPart2")]
    [HttpGet]
    public IActionResult RegisterPart2(RegisterViewModel model)
    {
        //return View("RegisterPart2", model);
        //if (!ModelState.IsValid)
        //{
        //    _logger.LogInformation($"Ошибка во второй части регистрации! ");
        //    return View("Shared/Register"); // Возврат на первую стадию при ошибках
        //}
        _logger.LogInformation($"Вы во второй части регистрации! ");
        return View("RegisterPart2", model); // Показ второй стадии
    }

    /// <summary>
    /// возврат на вторую форму
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    //[Route("Register")]
    //[HttpPost]
    //public async Task<IActionResult> Register(RegisterViewModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var user = _mapper.Map<User>(model);

    //        var result = await _userManager.CreateAsync(user, model.PasswordReg);
    //        if (result.Succeeded)
    //        {
    //            await _signInManager.SignInAsync(user, false);
    //            _logger.LogInformation($"Пользователь {user.UserName} успешно прошёл регистрацибю.");
    //            return RedirectToAction("MyPage", "AccountManager");
    //        }
    //        else
    //        {
    //            foreach (var error in result.Errors)
    //            {
    //                ModelState.AddModelError(string.Empty, error.Description);
    //            }
    //        }
    //    }
    //    return View("RegisterPart2", model);
    //    // Если ошибки - остаёмся на текущей стадии
    //    //return model.FirstName == null ? View("Shared/Register") : View("RegisterPart2", model);
    //}

    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        //if (!ModelState.IsValid)
        //{
        //    _logger.LogInformation($"ModelStateIsNOTValid in POST REGISTER");
        //    return View("~/Views/Shared/Register.cshtml", model);
        //}
        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    _logger.LogError($"Поле {entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            return View("~/Views/Shared/Register.cshtml", model);
        }

        model.Login = model.EmailReg;

        var user = _mapper.Map<User>(model);
        var result = await _userManager.CreateAsync(user, model.PasswordReg);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation($"Пользователь {user.UserName} успешно прошёл регистрацию.");
            return RedirectToAction("MyPage", "AccountManager");
        }

        // Если дошли сюда, значит были ошибки
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
            _logger.LogError(error.Description);
        }

        // Возвращаем на вторую страницу с ошибками
        return View("~/Views/RegisterPart2.cshtml", model);
    }
}
