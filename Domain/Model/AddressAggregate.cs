using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class AddressAggregate
    {
        public int Id { get; private set; }

        public int UserId { get; private set; }

        public string City { get; private set; }

        public string Street { get; private set; }

        [JsonIgnore]
        public virtual UserAggregate User { get; private set; }

        [JsonIgnore]
        public virtual List<OrderAggregate> Orders { get; private set; }

        public AddressAggregate()
        {
            //onlt db
        }

        public AddressAggregate(int userId, string city, string street)
        {
            UserId = userId;
            City = city;
            Street = street;
        }

        public static AddressAggregate Create(int userId, string city, string street)
        {
            return new AddressAggregate(userId, city, street);
        }

        public void Update(string city, string street)
        {
            City = city;
            Street = street;
        }
    }
}
