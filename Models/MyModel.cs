using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopM4.Models
{
    public class MyModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }
    }
}

