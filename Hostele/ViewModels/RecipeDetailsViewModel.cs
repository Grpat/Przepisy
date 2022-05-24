using System.ComponentModel.DataAnnotations;
using Hostele.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hostele.ViewModels;

public class RecipeDetailsViewModel
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public string RecipeName { get; set; }
    [Range(1, 200)]
    public int PrepTime { get; set; }
    
    [ValidateNever]
    public string? RecipeImage { get; set; }
    
    public Difficulty Difficulty { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public IEnumerable<Ingredient>? Ingrds { get; set; }
    public List<Step> Steps { get; set; }
    
    public string AppUserId { get; set; }
    
    public AppUser AppUser { get; set; }
}