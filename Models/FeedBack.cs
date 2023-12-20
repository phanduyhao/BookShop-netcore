using System;
using System.Collections.Generic;

namespace BookShop.Models
{
    public partial class FeedBack
    {
        public int IdFeedBack { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ContentFeed { get; set; }
    }
}
