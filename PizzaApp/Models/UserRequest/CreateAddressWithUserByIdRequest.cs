using Application.User.Commands;
using Domain.Model;

namespace ShopAPI.Models.UserRequest
{
    public class CreateAddressWithUserByIdRequest
    {
        public string City { get; set; }
        public string Street { get; set; }

        public AddAddressWithUserByIdCommand ToCommand(int id)
        {
            return new AddAddressWithUserByIdCommand(id, City, Street);
        }
    }
}
