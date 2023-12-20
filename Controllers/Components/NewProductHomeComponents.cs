using Microsoft.AspNetCore.Mvc;
using BookShop.Models;

namespace BookShop.Controllers.Components
{
    [ViewComponent(Name = "NewProductHome")]

    public class NewProductHomeComponents : ViewComponent
    {
        private bookstoreContext _context;
        public NewProductHomeComponents(bookstoreContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofNewProduct = (from m in _context.Products where( m.HomeFlag == true ) && (m.Active == true) select m).Take(8).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofNewProduct));
        }
    }
}
