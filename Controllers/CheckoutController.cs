using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookShop.Extension;
using BookShop.Helpper;
using BookShop.Models;
using BookShop.ModelViews;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly bookstoreContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(bookstoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
            }
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHang)
        {
            // Retrieve cart and customer ID from session
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();

            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;

                // Update customer's address if it's available in the model
                khachhang.Address = muaHang.Address;
                _context.Update(khachhang);
                _context.SaveChanges();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Create a new order
                    Order donhang = new Order();
                    donhang.CustomerId = model.CustomerId;
                    donhang.OrderDate = DateTime.Now;
                    donhang.OrderStatus2 = 1;
                    donhang.Deleted = false;
                    donhang.Paid = false;
                    donhang.Note = Utilities.StripHTML(model.Note);
                    donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney));

                    _context.Add(donhang);
                    _context.SaveChanges();

                    // Create order details for each item in the cart
                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Quanity = item.amount;
                        orderDetail.Total = donhang.TotalMoney;
                        orderDetail.Price = item.product.Price;
                        orderDetail.Shipdate = DateTime.Now;
                        _context.Add(orderDetail);
                    }

                    _context.SaveChanges();

                    // Clear the shopping cart from the session
                    HttpContext.Session.Remove("GioHang");

                    // Show success notification
                    _notyfService.Success("Đơn hàng đặt thành công");

                    // Redirect to the "Success" page
                    return RedirectToAction("Success");
                }
            }
            catch
            {
                // If there's an issue, return the same view with the cart
                ViewBag.GioHang = cart;
                return View(model);
            }

            // If ModelState is not valid, return the same view with the cart
            return RedirectToAction("Index","News");

        }

        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public IActionResult Success()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            ViewBag.GioHang = cart;

            /*try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                var donhang = _context.Orders
                    .Where(x => x.CustomerId == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.OrderDate)
                    .FirstOrDefault();
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = khachhang.FullName;
                successVM.DonHangID = donhang.OrderId;
                successVM.Phone = khachhang.Phone;
                successVM.Address = khachhang.Address;
                return View(successVM);
            }
            catch
            {*/
            return View();
/*            }*/
        }
       
    }
}