using FRUITABLE.Data;
using FRUITABLE.Helpers.Extensions;
using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using FRUITABLE.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;


        public ProductController(AppDbContext context, IProductService service, IWebHostEnvironment webHostEnvironment,
                                  ICategoryService categoryService)
        {
            _context = context;
            _productService = service;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var dbProduct = await _productService.GetAllPaginateAsync(page);

            List<ProductVM> mappedDatas = _productService.GetMappedDatas(dbProduct);

            int pageCount = await GetPageCountAsync(4);

            Paginate<ProductVM> model = new(mappedDatas, pageCount, page);

            return View(model);


        }
        private async Task<int> GetPageCountAsync(int take)
        {
            int count = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)count / take);
        }

        [HttpGet]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _productService.GetByIdAsync((int)id);
            if (product == null) return NotFound();

            List<ProductImageVM> productImages = new();
            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });

            }

            ProductDetailVM model = new()
            {
                Name = product.Name,

                Category = product.Category.Name,
                Images = productImages,
                Quality = product.Quality,
                Check = product.Check,
                MinWeight = product.MinWeight,
                Weight = product.Weight

            };
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await GetCategoriesAsync();
            return View();
        }

        private async Task<SelectList> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            return new SelectList(categories, "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.Categories = await _categoryService.GetAllBySelectedAsync();
            if (!ModelState.IsValid) return View();

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 kb");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "Image format must be img");
                    return View();
                }
            }
            List<ProductImages> images = new();

            foreach (var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                await item.SaveFileToLocalAsync(path);


                images.Add(new ProductImages
                {
                    Image = fileName
                });
            }

            images.FirstOrDefault().IsMain = true;

            Product product = new()
            {
                Name = request.Name,

                CategoryId = request.CategoryId,
                ProductImages = images,
                Weight = request.Weight,
                CountrOfOrigin = request.Origin,
                Check = request.Check,
                Quality = request.Quality

            };

            await _productService.CreateAsync(product);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _categoryService.GetAllBySelectedAsync();

            if (id == null) return BadRequest();
            Product product = await _productService.GetByIdAsync((int)id);
            if (product == null) return NotFound();


            List<ProductImageVM> productImage = new();

            foreach (var item in product.ProductImages)
            {
                productImage.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });
            }


            return View(new ProductEditVM { Name = product.Name, MinWeight = product.MinWeight, Images = productImage, Weight = product.Weight, Origin = product.CountrOfOrigin, Check = product.Check, Quality = product.Quality });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditVM productEditVM, int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return BadRequest();
            Product existProduct = await _productService.GetByIdAsync((int)id);
            if (existProduct == null) return NotFound();

            if (productEditVM.Photos != null)
            {
                foreach (var item in productEditVM.Photos)
                {
                    if (!item.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200 kb");
                        return View(productEditVM);
                    }
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Image format must be img");
                        return View(productEditVM);
                    }
                }

                foreach (var item in existProduct.ProductImages)
                {
                    _context.ProductImages.Remove(item);

                    FileExtensions.DeleteFileFromLocalAsync(Path.Combine(_webHostEnvironment.WebRootPath, "img"), item.Image);
                }

                List<ProductImages> images = new();

                foreach (var item in productEditVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                    await item.SaveFileToLocalAsync(path);

                    images.Add(new ProductImages
                    {
                        Image = fileName
                    });
                }

                images.FirstOrDefault().IsMain = true;

                existProduct.ProductImages = images;
            }

            existProduct.Name = productEditVM.Name;
            existProduct.Weight = productEditVM.Weight;
            existProduct.CountrOfOrigin = productEditVM.Origin;
            existProduct.Check = productEditVM.Check;
            existProduct.CategoryId = (int)productEditVM.CategoryId;


            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Where(m => m.Id == id).Include(m => m.ProductImages).FirstOrDefaultAsync();
            if (product == null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", item.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
