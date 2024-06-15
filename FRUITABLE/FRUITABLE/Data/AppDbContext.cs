using FRUITABLE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Slider { get; set; }
        public DbSet<SliderInfo> SliderInfo { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<FactFeatureContent> factFeatureContents { get; set; }

    }
}
