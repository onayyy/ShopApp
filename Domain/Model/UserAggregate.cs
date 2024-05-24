using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserAggregate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<AddressAggregate> Addresses { get; set; }

        [JsonIgnore]
        public virtual List<OrderAggregate> Orders { get; set; }

        public UserAggregate()
        {
            // only db
        }
        private UserAggregate(string name)
        {
            Name = name;
        }

        public static UserAggregate Create(string name)
        {
            return new UserAggregate(name);
        }

  
    }
}
