using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using FRUITABLE.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Controllers
{
    public class BasketController : Controller
    {

        private readonly IBasketService _basketService;
        private readonly IProductService _productService;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(IBasketService basketService, UserManager<AppUser> userManager, IProductService productService)
        {
            _basketService = basketService;
            _userManager = userManager;
            _productService = productService;

        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            Basket basket = await _basketService.GetByAppUserIdAsync(existUser.Id);

            List<BasketVM> basketVMs = new();

            if (basket != null)
            {
                foreach (var item in basket.BasketProducts)
                {
                    basketVMs.Add(new BasketVM()
                    {
                        Id = item.Id,
                        Image = item.Product.ProductImages.FirstOrDefault(m => m.IsMain)?.Image,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Quantity = item.Quantity,
                        TotalPrice = item.Product.Price * item.Quantity
                    });
                }
            }

            return View(basketVMs);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int? productId)
        {
            if (productId is null)
                return BadRequest();

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            Product existProduct = await _productService.GetByIdAsync((int)productId);

            if (existProduct is null)
                return NotFound();

            BasketProduct basketProduct = await _basketService
                .GetBasketProductByProductIdAndAppUserIdAsync((int)productId, existUser.Id);

            Basket basket = await _basketService.GetByAppUserIdAsync(existUser.Id);

            if (basketProduct != null)
            {
                basketProduct.Quantity++;
                await _basketService.SaveAsync();

                return Ok();
            }

            if (basket != null)
            {
                basket.BasketProducts.Add(new BasketProduct()
                {
                    ProductId = (int)productId,
                    Quantity = 1
                });

                await _basketService.SaveAsync();
                return Ok();
            }

            Basket newBasket = new()
            {
                AppUserId = existUser.Id,
            };

            await _basketService.AddAsync(newBasket);
            await _basketService.SaveAsync();

            BasketProduct newBasketProduct = new()
            {
                BasketId = newBasket.Id,
                ProductId = (int)productId,
                Quantity = 1
            };

            await _basketService.AddBasketProductAsync(newBasketProduct);
            await _basketService.SaveAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Increase(int id)
        {
            if (id == null)
                return RedirectToAction("NotFound", "Error");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketProduct basketProduct = await _basketService.GetBasketProductByIdAsync((int)id);

            if (basketProduct == null)
                return RedirectToAction("NotFound", "Error");

            basketProduct.Quantity++;
            await _basketService.SaveAsync();

            decimal totalPrice = basketProduct.Product.Price * basketProduct.Quantity;
            decimal total = await _basketService.GetTotalByAppUserIdAsync(existUser.Id);

            return Ok(new { totalPrice, total });
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int id)
        {
            if (id == null)
                return RedirectToAction("NotFound", "Error");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketProduct basketProduct = await _basketService.GetBasketProductByIdAsync((int)id);

            if (basketProduct == null)
                return RedirectToAction("NotFound", "Error");

            if (basketProduct.Quantity > 1)
            {
                basketProduct.Quantity--;
            }
            else
            {
                _basketService.DeleteBasketProduct(basketProduct);
                await _basketService.SaveAsync();
            }

            await _basketService.SaveAsync();

            decimal totalPrice = basketProduct.Product.Price * basketProduct.Quantity;
            decimal total = await _basketService.GetTotalByAppUserIdAsync(existUser.Id);

            return Ok(new { totalPrice, total });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("NotFound", "Error");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketProduct basketProduct = await _basketService.GetBasketProductByIdAsync((int)id);

            if (basketProduct == null)
                return RedirectToAction("NotFound", "Error");

            _basketService.DeleteBasketProduct(basketProduct);
            await _basketService.SaveAsync();

            return Ok(await _basketService.GetTotalByAppUserIdAsync(existUser.Id));
        }
    }
}
