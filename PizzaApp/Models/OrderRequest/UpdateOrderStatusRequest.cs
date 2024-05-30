using Application.Order.Commands;
using Domain.Model;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ShopAPI.Models.OrderRequest
{
    public class UpdateOrderStatusRequest
    {
        public OrderAggregate.OrderStatus OrderStatus { get; set; }

        public UpdateOrderStatusCommand ToCommand(int id)
        {
            return new UpdateOrderStatusCommand (id, OrderStatus);
        }
    }
}
