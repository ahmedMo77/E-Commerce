using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public class UpdateStoreOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
