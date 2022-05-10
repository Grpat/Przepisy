using System.ComponentModel.DataAnnotations;

namespace Hostele.Models;

public class Category
{
    [Key]
    [Required]
    public int Id { get; set; }
    
   
    public string CategoryName { get; set; }
    
    public IEnumerable<Recipe>? Recipes { get; set; }
}