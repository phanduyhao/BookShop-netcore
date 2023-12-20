using System;
using System.Collections.Generic;
using BookShop.Models;

namespace BookShop.ModelViews
{
    public class HomeViewVM
    {
        public List<News> ? TinTucs { get; set; }
        public List<ProductHomeVM> ? Products { get; set; }
    }
}
