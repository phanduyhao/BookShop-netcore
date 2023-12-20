using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using BookShop.Models;

namespace BookShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly bookstoreContext _context;
        public ProductController(bookstoreContext context)
        {
            _context = context;
        }
        [Route("shop.html", Name = "ShopProduct")]

        public IActionResult Index(int? page)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cate).FirstOrDefault();

                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 10;
                var lsproduct = _context.Products
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsproduct, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;

                var lsProduct2 = _context.Products.AsNoTracking()
                      .Where(x => x.CateId == product.CateId && x.Active == true && x.HomeFlag == true)
                      .OrderByDescending(x => x.DateCreated)
                      .Take(4)
                      .ToList();
                ViewBag.SanPham = lsProduct2;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [Route("danhmuc/{Alias}", Name = ("ListProduct"))] 
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cate).FirstOrDefault();
                var pageSize = 10;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
                var lsCates = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CateId == danhmuc.CateId)
                    .OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsCates, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCate = danhmuc;
                var lsProduct2 = _context.Products.AsNoTracking()
                     .Where(x => x.Active == true)
                     .OrderByDescending(x => x.DateCreated)
                     .Take(4)
                     .ToList();
                ViewBag.SanPham = lsProduct2;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(x => x.Cate).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var lsProduct = _context.Products.AsNoTracking()
                    .Where(x => x.ProductId != id && x.Active == true )
                    .OrderByDescending(x => x.DateCreated)
                    .Take(4)
                    .ToList();
                ViewBag.SanPham = lsProduct;

                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
