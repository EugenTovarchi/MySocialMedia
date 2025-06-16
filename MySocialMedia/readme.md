Приложение MVC .net9.0

Устанавливаем Nuget: 
	1.Microsoft.AspNetCore.Identity.EntityFrameworkCore(9.0.5)
	2.Microsoft.Entity.FrameworkCore(9.0.5)
	3.Microsoft.Entity.FrameworkCore.SqlServer(9.0.5)

Создаем сущность User.cs
Создаем папку Repositories > ApplicationDbContext.cs  

Настраиваем подключение к ДБ:
	-appsetings.json : 
	{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MySocialMedia;Trusted_Connection=True;"
  },

	-Program.cs : 
	builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer());

            //Настройки пароля через Identity
            builder.Services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

    app.UseAuthentication();
    app.UseAuthorization();

Теперь беремся за главную страницу: 



public IActionResult MyPage()
{
    // Получаем объект пользователя из контекста HTTP-запроса
    var user = User;  // User — это ClaimsPrincipal текущего пользователя

    // Асинхронно получаем данные пользователя из базы через UserManager
    var result = _userManager.GetUserAsync(user);

    // Создаём ViewModel и передаём её в представление "User"
    return View("User", new UserViewModel(result.Result));
}

User (тип ClaimsPrincipal)

Это автоматически заполняемый объект, содержащий информацию о текущем пользователе (его Claims — утверждения, например, Name, Role, Email).

Доступен только после успешной аутентификации (благодаря [Authorize]).

_userManager.GetUserAsync(user)

UserManager<TUser> ищет пользователя в БД на основе данных из User.

Обычно использует User.Identity.Name (логин) или User.FindFirstValue(ClaimTypes.NameIdentifier) (ID).

result.Result

Здесь происходит синхронное получение результата (await было бы лучше, но в примере используется .Result).

Если задача не завершена, это может привести к блокировке потока (риск deadlock в ASP.NET Core).

UserViewModel

Предназначен для передачи данных в представление (например, имя, почта, роли).

return View("User", ...)

Рендерит представление /Views/Account/User.cshtml (или другое, в зависимости от структуры проекта).

Передаёт туда UserViewModel.