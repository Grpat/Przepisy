using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostele.Models;

public class Ingredients
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    public string IngredientName { get; set; }
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Carbohydrates { get; set; }
    
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Protein { get; set; }
    
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Fat { get; set; }
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Kcal { get; set; }
    public IEnumerable<RecipeIngredient>? RecipeIngredients { get; set; }

}