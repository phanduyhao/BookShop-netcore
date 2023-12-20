using System;
using System.Collections.Generic;

namespace BookShop.Models
{
    public partial class News
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? ShortDesc { get; set; }
        public string? MaxDesc { get; set; }
        public string? Thumb { get; set; }
        public bool Published { get; set; }
        public string? Alias { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Author { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewFeed { get; set; }
        public string? MetaKey { get; set; }
        public string? MetaDesc { get; set; }
        public int? Views { get; set; }
        public int? AccountId { get; set; }
        public int? CateId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Category? Cate { get; set; }
    }
}
