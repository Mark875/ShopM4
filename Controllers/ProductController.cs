using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopM4.Data;
using ShopM4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4.Models.ViewModels;

namespace ShopM4.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;
        private IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product;

            /*
            foreach (var item in objList)
            {
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            */
            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            /*
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x => 
            new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList; */

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Category.Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = db.Product.Find(id);
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel.Product);
            }
        }

        // POST - CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ProductViewModel productViewModel)
        {
            var files = HttpContext.Request.Form.Files;
            string root = webHostEnvironment.WebRootPath;

            if (productViewModel.Product.Id == 0) 
            {
                // create
                string upload = root + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(upload + imageName + extension, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productViewModel.Product.Image = imageName + extension;

                db.Product.Add(productViewModel.Product);
            }
            else 
            {
                // update
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

