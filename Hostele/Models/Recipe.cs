using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hostele.Models;

public class Recipe
{ 
    [Key]
    [Required]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
    public string RecipeName { get; set; }
    [Range(1, 200)]
    public int PrepTime { get; set; }
    
    [ValidateNever]
    public string RecipeImage { get; set; }
    
    public Difficulty Difficulty { get; set; }
    public int? CategoryId{ get; set; }
    public Category? Category { get; set; }
    
    public IEnumerable<Ingredient>? Ingrds { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
    public List<Step>? Steps { get; set; }
    
    public string? AppUserId{ get; set; }
    [ForeignKey("AppUserId")]
    [ValidateNever]
    public AppUser? AppUser { get; set; }
    
    
    
}