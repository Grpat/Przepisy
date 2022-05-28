using System.ComponentModel.DataAnnotations.Schema;

namespace Hostele.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string IngredientName { get; set; }
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    [Column(TypeName = "decimal(5, 1)")]
    public decimal Amount { get; set; }
    public int MeasureNumber { get; set; }
    public Measure Measure { get; set; }
}