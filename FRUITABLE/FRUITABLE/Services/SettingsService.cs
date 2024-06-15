using FRUITABLE.Data;
using FRUITABLE.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly AppDbContext _context;
        public SettingsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}
