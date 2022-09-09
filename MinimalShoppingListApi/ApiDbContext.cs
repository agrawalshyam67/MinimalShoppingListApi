using Microsoft.EntityFrameworkCore;

namespace MinimalShoppingListApi
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Grocery> Groceries => Set<Grocery>();
    }
}
