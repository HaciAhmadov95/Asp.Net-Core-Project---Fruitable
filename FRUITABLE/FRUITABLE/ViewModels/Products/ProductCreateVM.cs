﻿using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.Products
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        [Required]

        public decimal MinWeight { get; set; }
        [Required]

        public decimal Weight { get; set; }
        [Required]

        public string Origin { get; set; }
        [Required]

        public string Quality { get; set; }
        [Required]

        public string Check { get; set; }
        [Required]
        public decimal Price { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
