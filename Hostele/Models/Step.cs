using System.ComponentModel.DataAnnotations;

namespace Hostele.Models;

public class Step
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public int? RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
}