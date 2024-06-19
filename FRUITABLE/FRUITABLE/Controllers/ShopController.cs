﻿using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using FRUITABLE.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ShopController(AppDbContext context, ICategoryService categoryService, IProductService productService)
        {
            _context = context;
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productService.GetAllAsync();
            List<Category> categories = await _categoryService.GetAllAsync();


            ShopVM model = new()
            {
                Products = products,
                Categories = categories
            };
            return View(model);
        }

        public async Task<IActionResult> Sorting(string sort)
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();

            switch (sort)
            {
                case "A-Z":
                    products = products.OrderBy(m => m.Name);
                    break;
                case "Z-A":
                    products = products.OrderByDescending(m => m.Name);
                    break;
                case "Old to New":
                    products = products.OrderBy(m => m.Id);
                    break;
                case "Cheap to Expensive":
                    products = products.OrderBy(m => m.Price);
                    break;
                case "Expensive to Cheap":
                    products = products.OrderByDescending(m => m.Price);
                    break;
            }

            ShopVM model = new() { Products = products };

            return PartialView("_ProductFilterPartial", model);
        }


        public async Task<IActionResult> Search(string searchText)
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();

            products = searchText != null
                ? products.Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                : products.Take(6);

            ShopVM model = new() { Products = products };

            return PartialView("_ProductFilterPartial", model);
        }

        public async Task<IActionResult> CategoryFilter(int id)
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();

            ShopVM model = new() { Products = products.Where(m => m.CategoryId == id) };

            return PartialView("_ProductFilterPartial", model);
        }

        public async Task<IActionResult> PriceFilter(decimal price)
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();

            ShopVM model = new()
            {
                Products = price > 0 ? products.Where(m => m.Price <= price) : products
            };

            return PartialView("_ProductFilterPartial", model);
        }
    }
}