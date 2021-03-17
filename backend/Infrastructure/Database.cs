using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure
{
    public class Database : DbContext
    {
        public DbSet<Pokemon> Pokemon { get; set; }
    }
}
