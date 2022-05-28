using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Hostele.Models;

public class Comment
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
    public string UserName { get; set; }
    
    public int Rating { get; set; }
    public int? RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    
    
    public string AppUserId { get; set; }
    [ForeignKey("AppUserId")]
    [ValidateNever]
    public AppUser AppUser { get; set; }

}