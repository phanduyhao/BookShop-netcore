using System;
using System.Collections.Generic;

namespace BookShop.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderStatus1 { get; set; }
        public string Status { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
