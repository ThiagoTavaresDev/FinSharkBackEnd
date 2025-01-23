using FinSharkProjeto.Model;
using Microsoft.EntityFrameworkCore;

namespace FinSharkProjeto.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}