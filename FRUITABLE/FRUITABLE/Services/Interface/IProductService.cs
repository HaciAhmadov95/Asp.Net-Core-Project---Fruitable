﻿using FRUITABLE.Models;
using FRUITABLE.ViewModels.Products;

namespace FRUITABLE.Services.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task<List<Product>> GetAllPaginateAsync(int page, int take = 4);
        List<ProductVM> GetMappedDatas(List<Product> products);
        Task<int> GetCountAsync();
    }
}