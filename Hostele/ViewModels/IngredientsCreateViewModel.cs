using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hostele.Models;

namespace Hostele.ViewModels;

public class IngredientsCreateViewModel
{
    [Key]
    [Required]
    public int Id { get; set; }
    public Guid UniqueId { get; set; } = Guid.NewGuid();
    
    [Display(Name = "Składnik potrawy")]
    public string IngredientName { get; set; }
    
    [Display(Name = "Węglowodany")]
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Carbohydrates { get; set; }
    
    [Display(Name = "Białko")]
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Protein { get; set; }
    
    [Display(Name = "Tłuszcz")]
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Fat { get; set; }
    
    [Display(Name = "Kcal")]
    [Column(TypeName = "decimal(5, 1)")]
    public decimal? Kcal { get; set; }
   
}