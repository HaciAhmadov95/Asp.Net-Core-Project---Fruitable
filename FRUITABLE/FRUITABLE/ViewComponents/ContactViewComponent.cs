using FRUITABLE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly ISettingsService _settingService;
        public ContactViewComponent(ISettingsService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(await _settingService.GetAllAsync()));
        }
    }
}
