
using FRUITABLE.Models;
using FRUITABLE.ViewModels.Categories;
using FRUITABLE.ViewModels.CategoryVM;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FRUITABLE.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<List<CategoryVM>> GetAllOrderByDescendingAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(CategoryCreateVM category);
        Task<Category> GetWithProductAsync(int id);
        Task DeleteAsync(Category category);
        Task<Category> GetByIdAsync(int id);
        Task EditAsync(Category category, CategoryEditVM categoryEdit);
        Task<SelectList> GetAllBySelectedAsync();

    }
}
