using Microsoft.EntityFrameworkCore;
using ArabaKiralama.Models;

namespace ArabaKiralama.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
    }
}