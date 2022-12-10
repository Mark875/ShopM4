using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Models.ViewModels;

namespace ShopM4.Controllers;

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
        DetailsViewModel detailsViewModel = new DetailsViewModel()
        {
            IsEmpty = true,
            Product = db.Product.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault()
        };
        return View(detailsViewModel);
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
