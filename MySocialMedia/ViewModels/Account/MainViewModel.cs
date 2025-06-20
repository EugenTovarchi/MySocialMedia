namespace MySocialMedia.ViewModels.Account
{
    public class MainViewModel
    {
        public LoginViewModel LoginView { get; set; }
        public RegisterViewModel RegisterView { get; set; }

        public MainViewModel()
        {
            LoginView = new LoginViewModel();
            RegisterView = new RegisterViewModel();
        }

        public string FullName { get; set; }
    }
}
