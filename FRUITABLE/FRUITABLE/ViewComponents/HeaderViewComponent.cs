using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using FRUITABLE.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingsService _settingsService;
        private readonly IBasketService _basketService;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(ISettingsService settingsService, IBasketService basketService, UserManager<AppUser> userManager)
        {
            _settingsService = settingsService;
            _basketService = basketService;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser existUser = new();

            if (User.Identity.IsAuthenticated)
            {
                existUser = await _userManager.FindByNameAsync(User.Identity?.Name);
            }

            HeaderVM model = new()
            {
                Settings = await _settingsService.GetAllAsync(),
                BasketCount = await _basketService.GetCountByAppUserIdAsync(existUser.Id)
            };

            return await Task.FromResult(View(model));
        }
    }
}
