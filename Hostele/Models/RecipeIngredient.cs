using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostele.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int IngredientsId { get; set; }
    public Ingredients? Ingredients{ get; set; }
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Amount { get; set; }
    public int? MeasureNumber { get; set; }
    public Measure? Measure { get; set; }
}