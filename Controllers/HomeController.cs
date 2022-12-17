using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Models.ViewModels;
using System.Collections.Generic;
using System;

namespace ShopM4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            // view
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = db.Product,
                Categories = db.Category
            };
            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                IsEmpty = true,
                Product = db.Product.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault()
            };

            foreach (var item in cartList)
            {
                if (item.ProductId == id)
                {
                    detailsViewModel.IsEmpty = false;
                }
            }

            return View(detailsViewModel);
        }

        [HttpPost]
        public IActionResult RemoveFormCart(int id)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }

            for (int i = 0; i < cartList.Count; i++)
            {
                if (cartList[i].ProductId == id)
                {
                    cartList.RemoveAt(i);
                    i--;
                }
            }

            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DetailsPost(int id)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && 
                HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }

            cartList.Add(new Cart() { ProductId = id });

            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    /*
     */

}

