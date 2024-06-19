using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Services
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BasketProduct> GetBasketProductByProductIdAndAppUserIdAsync(int productId, string appUserId)
        {
            return await _context.basketProducts.FirstOrDefaultAsync(m => m.ProductId == productId && m.Basket.AppUserId == appUserId);
        }

        public async Task<Basket> GetByAppUserIdAsync(string appUserId)
        {
            return await _context.Basket
                .Include(m => m.BasketProducts)
                .ThenInclude(m => m.Product)
                .ThenInclude(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.AppUserId == appUserId);
        }

        public async Task AddAsync(Basket basket)
        {
            await _context.Basket.AddAsync(basket);
        }

        public async Task AddBasketProductAsync(BasketProduct basketProduct)
        {
            await _context.basketProducts.AddAsync(basketProduct);
        }

        public void DeleteBasketProduct(BasketProduct basketProduct)
        {
            _context.basketProducts.Remove(basketProduct);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountByAppUserIdAsync(string appUserId)
        {
            return await _context.basketProducts.Where(m => m.Basket.AppUserId == appUserId)
                 .SumAsync(m => m.Quantity);
        }

        public async Task<decimal> GetTotalByAppUserIdAsync(string appUserId)
        {
            return await _context.basketProducts.Where(m => m.Basket.AppUserId == appUserId)
                 .SumAsync(m => m.Product.Price * m.Quantity);
        }

        public async Task<BasketProduct> GetBasketProductByIdAsync(int id)
        {
            return await _context.basketProducts.Include(m => m.Product).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
