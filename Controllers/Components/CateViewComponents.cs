using Microsoft.AspNetCore.Mvc;
using BookShop.Models; 
namespace BookShop.Components
{
    [ViewComponent(Name ="CateView")]
    public class CateViewComponents : ViewComponent
    {
        private bookstoreContext _context;
        public CateViewComponents(bookstoreContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listofCate = (from m in _context.Categories select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default",listofCate));
        }
    }
}
