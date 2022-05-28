﻿namespace Hostele.Repository;

public interface IUnitOfWork
{
    
   IRecipeRepository Recipe{ get;}
   ICategoryRepository Category{ get;}
   ICommentRepository Comment{ get;}
    Task Save();
}