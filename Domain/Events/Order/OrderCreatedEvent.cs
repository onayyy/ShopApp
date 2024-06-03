using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Order
{
    public class OrderCreatedEvent
    {
        public List<int> ProductIds { get; set; } = new List<int>();


    }
}
