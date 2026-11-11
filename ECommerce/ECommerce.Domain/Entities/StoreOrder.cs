using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class StoreOrder
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Store Store { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}
