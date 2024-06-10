using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
