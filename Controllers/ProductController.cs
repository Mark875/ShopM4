using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopM4.Data;
using ShopM4.Models;
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

            // получаем ссылки на сущности категорий
            /*
            foreach (var item in objList)
            {
                // сопоставление таблицы категорий и таблицы product
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            */

            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            /*
            // получаем лист категорий для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            // отправляем лист категорий во View
            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList;
            */

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                MyModelList = db.MyModel.Select(x =>
                new SelectListItem
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
                return View(productViewModel);
            }
        }


        // POST - CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ProductViewModel productViewModel)
        {
            var files = HttpContext.Request.Form.Files;

            string wwwRoot = webHostEnvironment.WebRootPath;

            if (productViewModel.Product.Id == 0)
            {
                // create
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                string extension = Path.GetExtension(files[0].FileName);

                string path = upload + imageName + extension;

                // скопируем файл на сервер
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productViewModel.Product.Image = imageName + extension;

                db.Product.Add(productViewModel.Product);
            }
            else
            {
                // update
                // AsNoTracking() - IMPORTANT!!!
                var product = db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.Product.Id); // LINQ

                if (files.Count > 0)  // юзер загружает другой файл
                {
                    string upload = wwwRoot + PathManager.ImageProductPath;
                    string imageName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;

                    // delete old file
                    var oldFile = upload + product.Image;

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    // скопируем файл на сервер
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = imageName + extension;
                }
                else   // фотка не поменялась
                {
                    productViewModel.Product.Image = product.Image;  // оставляем имя прежним
                }

                db.Product.Update(productViewModel.Product);
            }

            db.SaveChanges();

            return RedirectToAction("Index");

            //return View();
        }


        // GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = db.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Category = db.Category.Find(product.CategoryId);

            return View(product);
        }

        // POST
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // delete from db
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();

            // delete image from server
            string upload = webHostEnvironment.WebRootPath + PathManager.ImageProductPath;

            // получаем ссылку на нашу старую фотку
            var oldFile = upload + product.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            return RedirectToAction("Index");
        }

    }
}