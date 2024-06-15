using FRUITABLE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingsService _settingsService;

        public FooterViewComponent(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(await _settingsService.GetAllAsync()));
        }
    }

}

