using System;

namespace ShopM4.Models.ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; set; }
        public bool IsEmpty { get; set; }
        public DetailsViewModel() 
        { 
            Product = new Product();
        }
    }
}
