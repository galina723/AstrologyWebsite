namespace AstrologyWebsite.ViewModels
{
    public class AccountPageViewModel
    {
        public LoginViewModel Login { get; set; } = new();
        public RegisterViewModel Register { get; set; } = new();
    }
}
