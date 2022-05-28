using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hostele.Models;

namespace Hostele.ViewModels;

public class IngredientCreateViewModel
{
    public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    
    
    public string IngredientName { get; set; }
    
    [Column(TypeName = "decimal(5, 1)")]
    public decimal Amount { get; set; }
    /*public int MeasureNumber { get; set; }*/
    public Measure? Measure { get; set; }
    
}