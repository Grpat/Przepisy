using System.Collections.Immutable;
using AutoMapper;
using Hostele.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Policy;
using Hostele.ViewModels;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Hostele.Controllers;
using Hostele.Models;
using Hostele.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hostele.Controllers
{
    
    public class UserRecipeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserRecipeController(IWebHostEnvironment hostEnvironment, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _unitOfWork.Recipe.GetAll2(x=>x.AppUserId== userId));
        }

        
        public async Task<IActionResult>  Details(int? id)
        {
            
            return RedirectToAction("RecipeDetails", "Recipe",new {id=id});
        }
        
        
        
        
        
        public async Task<IActionResult> DeleteIngr(int? id)
        {
           
            var ingredient = await _unitOfWork.Ingredient.GetFirstOrDefault(filter: m => m.Id == id);
            
            if (id == null)
            {
                return NotFound();
            }
            

            return View(ingredient);
            
        }
        
        [HttpPost, ActionName("IngrDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IngrDeleteConfirmed(int? id,int RecipeId)
        {
            
            var ingredient = await _unitOfWork.Ingredient.GetFirstOrDefault(filter: m => m.Id == id);
            
            _unitOfWork.Ingredient.Remove(ingredient);
            await _unitOfWork.Save();
            return RedirectToAction("UserRecipeDetails","UserRecipe",new {id=RecipeId});
        }

        
        
        
        
        
        
        
        public async Task<IActionResult> DeleteStep(int? id)
        {
           
            var step = await _unitOfWork.Step.GetFirstOrDefault(filter: m => m.Id == id);
            
            if (id == null)
            {
                return NotFound();
            }
            

            return View(step);
            
        }
        
        [HttpPost, ActionName("StepDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StepDeleteConfirmed(int? id,int RecipeId)
        {
            
            var recipe = await _unitOfWork.Step.GetFirstOrDefault(filter: m => m.Id == id);
            
            _unitOfWork.Step.Remove(recipe);
            await _unitOfWork.Save();
            return RedirectToAction("UserRecipeDetails","UserRecipe",new {id=RecipeId});
        }

        
        public async Task<IActionResult> UserRecipeDetails(int? id)
        {
          

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(filter: x => x.Id == id,includeProperties: "Category,Steps,Ingrds,Comments");
            var userRecipeDetailsEditViewModel = _mapper.Map<UserRecipeEditViewModel>(recipe);
            
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Id", recipe.CategoryId);
            return View(userRecipeDetailsEditViewModel);
        }

        // POST: AddRecipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> UserRecipeDetails(int id,[Bind("Id,RecipeName,PrepTime,ImageVm,RecipeImage,Difficulty,CategoryId,Steps,Ingrds")] UserRecipeEditViewModel recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                string wwwRootPath=_hostEnvironment.WebRootPath;
                if (recipe.ImageVm != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\recipes");
                    var extension = Path.GetExtension(recipe.ImageVm.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        await recipe.ImageVm.CopyToAsync(fileStreams);
                    }

                   
                    recipe.RecipePath = @"\images\recipes\" + fileName + extension;
                    
                }
                var Recipe = _mapper.Map<Recipe>(recipe);
                Recipe.AppUserId = claim.Value;
                await _unitOfWork.Recipe.UpdateRecipe(Recipe);
                await _unitOfWork.Save();
                return RedirectToAction("RecipeDetails", "Recipe",new {id=id});
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Id", recipe.CategoryId);
            return View(recipe);
        }

       
        public async Task<PartialViewResult> DisplaySteps()
        
        {
            var step = new StepCreateViewModel();
            return PartialView("_StepPartialEdit",step);
        }  
        
        public async Task<PartialViewResult> DisplayIngredient()
        
        {
            var ingredient = new IngredientCreateViewModel();
            return PartialView("_RecipeIngredientPartiallEdit2",ingredient);
        }  
        
        private bool RecipeExists(int id)
        {
            return _unitOfWork.Recipe.CheckIfExists(x => x.Id == id);
        }
        
        
        
    }
}
