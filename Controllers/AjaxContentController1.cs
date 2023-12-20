using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Controllers
{
    public class AjaxContentController1 : Controller
    {
        public IActionResult HeaderCart()
        {
            return ViewComponent("HeaderCart");
        }
        public IActionResult HeaderFavourites()
        {
            return ViewComponent("NumberCart");
        }
    }
}
