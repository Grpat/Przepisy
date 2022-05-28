using System.Runtime.CompilerServices;
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
        modelBuilder.Entity<Comment>()
            .HasOne(b => b.Recipe)
            .WithMany(a => a.Comments)
            .HasForeignKey(u => u.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Step>()
            .HasOne(b => b.Recipe)
            .WithMany(a => a.Steps)
            .HasForeignKey(u => u.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*modelBuilder.Entity<Recipe>()
            .HasMany(b => b.Comments)
            .WithOne(a => a.Recipe)
            .OnDelete(DeleteBehavior.Cascade);*/
        
        modelBuilder.Entity<Recipe>()
            .HasOne(b => b.Category)
            .WithMany(a => a.Recipes)
            .HasForeignKey(u => u.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
       
        
       
            
            
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredients> Ingredients{ get; set; }
    
    public DbSet <Ingredient> Ingrds { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Comment> Comments { get; set; }
  
    
}