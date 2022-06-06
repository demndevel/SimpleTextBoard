using Demnoboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Demnoboard;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Post> Posts { get; set; }
    
    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}