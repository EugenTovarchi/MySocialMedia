using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySocialMedia.Models;
using MySocialMedia.Models.Users;
using MySocialMedia.ViewModels.Account;

namespace MySocialMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Route("")]
        [Route("[controller]/[action]")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(User).Result;
                var model = new MainViewModel
                {
                    FullName = $"{user?.FirstName} {user?.LastName}"
                };
                return View(new MainViewModel());
            }
            else
            {
                return View(new MainViewModel { FullName = "Гость" });
            }
           
        }

        [Route("[action]")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
