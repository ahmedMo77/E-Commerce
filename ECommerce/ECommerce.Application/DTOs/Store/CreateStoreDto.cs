using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Store
{
    public class CreateStoreDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
