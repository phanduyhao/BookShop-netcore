using Microsoft.AspNetCore.Mvc;
using BookShop.Models;

namespace BookShop.Controllers.Components
{
    [ViewComponent(Name = "HotProductHome")]

    public class HotProductHomeComponents : ViewComponent
    {
        private bookstoreContext _context;
        public HotProductHomeComponents(bookstoreContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofHotProduct = (from m in _context.Products where (m.Discount != 0) && (m.Active == true) select m).Take(6).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofHotProduct));
        }
    }
}
