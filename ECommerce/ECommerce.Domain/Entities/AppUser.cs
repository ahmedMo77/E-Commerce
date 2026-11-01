using ECommerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

        public Store? Store { get; set; }
        public Cart? Cart { get; set; }

        public ICollection<Order> Orders { get; set; } 
            = new List<Order>();
    }
}
