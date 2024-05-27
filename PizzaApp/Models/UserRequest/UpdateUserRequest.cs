using Application.User.Commands;

namespace ShopAPI.Models.UserRequest
{
    public class UpdateUserRequest
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public int Gender { get; set; }

        public UpdateUserCommand ToCommand(int id)
        {
            return new UpdateUserCommand(id, Name, Surname, Password, Gender);
        }
    }
}
