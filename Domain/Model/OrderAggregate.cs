using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class OrderAggregate
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int AddressId { get; private set; }
        public string OrderNumber { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string CustomerName { get; private set; }
        public virtual List<ProductAggregate> Products { get; private set; }
        public UserAggregate User { get; private set; }
        public AddressAggregate Address { get; private set; }


        public OrderAggregate()
        {
            //only db
        }

        public OrderAggregate(string orderNumber, double totalAmount, double discountAmount, string customerName, List<ProductAggregate> products, UserAggregate user, AddressAggregate address)
        {
            UserId = user.Id;
            AddressId = address.Id;
            OrderNumber = orderNumber;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            OrderDate = DateTime.Now;
            CustomerName = customerName;
            Products = products;
            User = user;
            Address = address;
        }

        public static OrderAggregate Create(string orderNumber, double totalAmount, double discountAmount, string customerName, List<ProductAggregate> products, UserAggregate user, AddressAggregate address)
        {
            return new OrderAggregate(orderNumber, totalAmount, discountAmount, customerName, products, user, address);
        }
    }
}
