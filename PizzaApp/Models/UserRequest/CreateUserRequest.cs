using Application.Product.Commands.ProductCommand;
using Application.Product.Commands.UserCommand;
using Domain.Model;

namespace ShopAPI.Models.UserRequest
{
    public class CreateUserRequest
    {
        public AddUserCommand ToCommand(string name)
        {
            return new AddUserCommand(name);
        }
    }
}
