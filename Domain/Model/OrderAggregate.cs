using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class OrderAggregate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string OrderNumber { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public virtual List<ProductAggregate> Products { get; set; }
        public UserAggregate User { get; set; }
        public AddressAggregate Address { get; set; }


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

        public void OrderStatusUpdate(OrderStatus orderStatus)
        {
            Status = orderStatus;
        }

        public enum OrderStatus
        {
            [Description("None")]
            None = 0,
            [Description("Sipariş Alındı")]
            SiparisAlindi = 1,
            [Description("Sipariş Hazırlanıyor")]
            SiparisHazirlaniyor = 2,
            [Description("Sipariş Yolda")]
            SiparisYolda = 3,
            [Description("Sipariş Teslim Edildi")]
            SiparisTeslimEdildi = 4
        }
    }
}
