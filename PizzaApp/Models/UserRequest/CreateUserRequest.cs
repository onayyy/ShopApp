using Application.Product.Commands.ProductCommand;
using Application.User.Commands;
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
