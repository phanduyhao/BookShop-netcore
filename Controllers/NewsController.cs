using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using BookShop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShop.Controllers
{
    public class NewsController : Controller
    {
        private readonly bookstoreContext _context;
        public NewsController(bookstoreContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        [Route("Tin-tuc.html", Name = ("New"))]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsNews = _context.News
                .AsNoTracking()
                .OrderBy(x => x.PostId);
            PagedList<News> models = new PagedList<News>(lsNews, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinChiTiet")]
        public IActionResult Details(int id)
        {
            var News = _context.News.AsNoTracking().SingleOrDefault(x => x.PostId == id);
            if (News == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaivietlienquan = _context.News
                 .AsNoTracking()
                 .Where(x => x.Published == true && x.PostId != id)
                 .Take(3)
                 .OrderByDescending(x => x.CreateDate).ToList();
            ViewBag.Baivietlienquan = lsBaivietlienquan;
            return View(News);
        }
    }
}
