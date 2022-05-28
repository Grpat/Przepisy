using System.ComponentModel.DataAnnotations;
using Hostele.Models;
using Hostele.Utility;
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
    public string RecipePath { get; set; }
    [MaxFileSize(5* 1024 * 1024)]
    [AllowedExtensions(new string[] { ".jpg", ".png" })]
    [DataType(DataType.Upload)]
    public IFormFile RecipeImage { get; set; }
    public IEnumerable<StepCreateViewModel> Steps { get; set; }
    public IEnumerable<IngredientCreateViewModel> Ingrds{ get; set; }
    
   
   
}