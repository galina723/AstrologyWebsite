using AstrologyWebsite.DTOs;

namespace AstrologyWebsite.ViewModels
{
    public class AccountViewPageModel
    {
        public LoginDto Login { get; set; } = new LoginDto();
        public RegisterDto Register { get; set; } = new RegisterDto();
    }
}
