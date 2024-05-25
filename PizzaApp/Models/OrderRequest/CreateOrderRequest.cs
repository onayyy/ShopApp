using Application.Order.Commands;
using Domain.Model;

namespace ShopAPI.Models.OrderRequest
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public required string CustomerName { get; set; }
        public required List<int> ProductIds { get; set; }

        public AddOrderCommand ToCommand()
        {
            return new AddOrderCommand(UserId, AddressId, CustomerName, ProductIds);
        }
    }
}
