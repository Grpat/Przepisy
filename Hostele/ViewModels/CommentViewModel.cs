using System.ComponentModel.DataAnnotations;

namespace Hostele.ViewModels;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
    
    [Range(1, 5, ErrorMessage = "Oceń potrawę w skali 1 do 5 gwiazdek ")]  
    public int Rating { get; set; }
    public int RecipeId { get; set; }
    
    
   
}