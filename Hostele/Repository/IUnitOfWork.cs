namespace Hostele.Repository;

public interface IUnitOfWork
{
    
   IRecipeRepository Recipe{ get;}
    Task Save();
}