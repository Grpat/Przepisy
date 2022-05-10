using Hostele.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hostele.ViewModels;

public class RecipeCreateViewModel
{
    
    public string RecipeName { get; set; }
    public int PrepTime { get; set; }
    
    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public int Difficulty { get; set; }
    public int CategoryId { get; set; }
    [ValidateNever]
    public string RecipeImage { get; set; }
    public IEnumerable<StepCreateViewModel> Steps { get; set; }
    public IEnumerable<RecipeIngredientCreateViewModel> RecipeIngredients { get; set; }
    
   
   
}