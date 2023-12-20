using System;
using System.Collections.Generic;

namespace BookShop.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDesc { get; set; }
        public string? Description { get; set; }
        public int? CateId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string? Thumb { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public bool BestSeller { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public int? UnitsInstock { get; set; }

        public virtual Category? Cate { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
