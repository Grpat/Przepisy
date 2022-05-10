namespace Hostele.ViewModels;

public class StepCreateViewModel

{
    public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    public string Description { get; set; }
    
}