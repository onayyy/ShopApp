using Application.Authentication.Commands;

namespace ShopAPI.Models.AuthenticationRequest
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }


        public LoginCommand ToCommand()
        {
            return new LoginCommand(Email, Password);
        }
    }
}
