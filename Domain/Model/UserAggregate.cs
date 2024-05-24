using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class UserAggregate
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public virtual List<AddressAggregate> Addresses { get; set; }

        public virtual List<OrderAggregate> Orders { get; set; }
    }
}
