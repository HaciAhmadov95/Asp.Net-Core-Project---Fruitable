using FRUITABLE.Models;

namespace FRUITABLE.Services.Interface
{
    public interface IBasketService
    {
        Task SaveAsync();
        Task AddAsync(Basket basket);
        Task AddBasketProductAsync(BasketProduct basketProduct);
        void DeleteBasketProduct(BasketProduct basketProduct);
        Task<Basket> GetByAppUserIdAsync(string appUserId);
        Task<int> GetCountByAppUserIdAsync(string appUserId);
        Task<decimal> GetTotalByAppUserIdAsync(string appUserId);
        Task<BasketProduct> GetBasketProductByIdAsync(int id);
        Task<BasketProduct> GetBasketProductByProductIdAndAppUserIdAsync(int productId, string appUserId);
    }
}
