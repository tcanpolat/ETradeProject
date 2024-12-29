﻿using ETICARET.Entities;
using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 5,ErrorMessage = "Ürün adı min 5 max 50 karakter olmalıdır.")]
        public string Name { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Ürün açıklaması min 5 max 200 karakter olmalıdır.")]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<Image> Images { get; set; }

        public List<Category> SelectedCategories { get; set; }
        public string CategoryId { get; set; }
        public ProductModel()
        {
            Images = new List<Image>();
        }
    }
}