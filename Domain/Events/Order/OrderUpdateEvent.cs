using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Order
{
    public class OrderUpdateEvent
    {
        public string Defination { get; set; }

        public string Subject { get; set; }

    }
}
