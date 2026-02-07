using JJK_API.Model;
using Microsoft.EntityFrameworkCore;

namespace JJK_API.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions options): base(options) { }

    public DbSet<User> Usuario { get; set;   }
  }
}
