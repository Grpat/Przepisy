using Hostele.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hostele.Data;

public class ApplicationDbContext:IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredients> Ingredients{ get; set; }
    
    public DbSet <Ingredient> Ingrds { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
  
    
}