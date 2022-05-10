using System.ComponentModel.DataAnnotations;
using Hostele.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hostele.ViewModels;

public class RecipeIngredientCreateViewModel
{
    public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    public int IngredientsId { get; set; }
    [Range(1, 1000)] 
    public int Amount { get; set; }
    public int MeasureNumber { get; set; }
    public Measure? Measure { get; set; } 
    public IEnumerable<SelectListItem>? Ingredients { get; set; }
}