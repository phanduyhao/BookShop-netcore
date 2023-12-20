using System;
using System.Collections.Generic;

namespace BookShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? ShipDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool OrderStatus { get; set; }
        public bool Deleted { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentId { get; set; }
        public string? Note { get; set; }
        public int? OrderStatus2 { get; set; }
        public int? TotalMoney { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual OrderStatus? OrderStatus2Navigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
