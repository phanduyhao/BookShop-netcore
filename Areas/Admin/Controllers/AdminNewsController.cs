using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using BookShop.Helpper;
using BookShop.Models;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminNewsController : Controller
    {
        private readonly bookstoreContext _context;
        public INotyfService _notyfService { get; }

        public AdminNewsController(bookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminNews
        public IActionResult Index(int? page)
        {
            var ls = _context.News.AsNoTracking().ToList();
            foreach(var item in ls)
            {
                if(item.CreateDate == null)
                {
                    item.CreateDate = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var Pagesize = 20;
            var IsNews = _context.News.AsNoTracking().OrderByDescending(x => x.PostId);
            PagedList<News> models = new PagedList<News>(IsNews, pageNumber, Pagesize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Account)
                .Include(n => n.Cate)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: Admin/AdminNews/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            return View();
        }

        // POST: Admin/AdminNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,ShortDesc,MaxDesc,Thumb,Published,Alias,CreateDate,Author,AccountId,CateId,IsHot,IsNewFeed,MetaKey,MetaDesc,Views")] News news, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(news.Title) + extension;
                    news.Thumb = await Utilities.UploadFile(fThumb, @"new", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(news.Thumb)) news.Thumb = "default.jpg";
                news.Alias = Utilities.SEOUrl(news.Title);
                news.CreateDate = DateTime.Now;
                _context.Add(news);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm thành công!");

                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", news.AccountId);
            return View(news);
        }

        // GET: Admin/AdminNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", news.AccountId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", news.CateId);
            return View(news);
        }

        // POST: Admin/AdminNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,ShortDesc,MaxDesc,Thumb,Published,Alias,CreateDate,Author,AccountId,CateId,IsHot,IsNewFeed,MetaKey,MetaDesc,Views")] News news, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != news.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(news.Title) + extension;
                        news.Thumb = await Utilities.UploadFile(fThumb, @"new", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(news.Thumb)) news.Thumb = "default.jpg";
                    news.Alias = Utilities.SEOUrl(news.Title);
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công!");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", news.AccountId);
            ViewData["CateId"] = new SelectList(_context.Categories, "CateId", "CateId", news.CateId);
            return View(news);
        }

        // GET: Admin/AdminNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Account)
                .Include(n => n.Cate)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/AdminNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'bookstoreContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công!");

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return _context.News.Any(e => e.PostId == id);
        }
    }
}
