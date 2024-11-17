using Microsoft.EntityFrameworkCore;
using FirstReadAPI.Models;

namespace FirstReadAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
    }
}
