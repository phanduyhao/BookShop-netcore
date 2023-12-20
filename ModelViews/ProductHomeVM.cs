using System;
using System.Collections.Generic;
using BookShop.Models;

namespace BookShop.ModelViews
{
    public class ProductHomeVM
    {
        public Category ? category { get; set; }
        public List<Product> ? lsProducts { get; set; }
    }
}
