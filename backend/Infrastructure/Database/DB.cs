using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Database
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options)
            : base(options)
        {
        }

        public DbSet<Pokemon> Pokemon { get; set; }
    }
}
