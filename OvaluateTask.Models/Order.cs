using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvaluateTask.Models
{
    public class Order
    {
        public Guid Id { get; set; } 
        public Guid CustomerId { get; set; } 
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public Customer ShipingAddress { get; set; }
        public Customer Customer { get; set; }
    }
}
