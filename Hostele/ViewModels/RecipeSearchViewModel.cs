using System.ComponentModel.DataAnnotations;
using Hostele.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hostele.ViewModels;

public class RecipeSearchViewModel
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public string RecipeName { get; set; }
  
    public int PrepTime { get; set; }
    
    public string? RecipeImage { get; set; }
    
    public Difficulty Difficulty { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    
}