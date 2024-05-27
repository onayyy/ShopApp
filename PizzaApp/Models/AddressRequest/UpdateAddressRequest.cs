using Application.Address.Commands;

namespace ShopAPI.Models.AddressRequest
{
    public class UpdateAddressRequest
    {
        public string City { get; set; }
        public string Street { get; set; }

        public UpdateAddressCommand ToCommand(int id)
        {
            return new UpdateAddressCommand(id, City, Street);
        }
    }
}
