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

