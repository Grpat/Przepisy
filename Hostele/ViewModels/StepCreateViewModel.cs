namespace Hostele.ViewModels;

public class StepCreateViewModel

{
    public int RecipeId { get; set; }
    public int? Id { get; set; }
    public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    public string Description { get; set; }
    
}