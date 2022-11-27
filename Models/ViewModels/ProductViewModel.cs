using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ShopM4.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategoriesList { get; set; }
    }
}
