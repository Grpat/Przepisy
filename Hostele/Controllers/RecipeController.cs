#nullable disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostele.Data;
using Hostele.Models;
using Hostele.Repository;
using Hostele.Utility;
using Hostele.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using NinjaNye.SearchExtensions;
using X.PagedList;


namespace Hostele.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public RecipeController(IWebHostEnvironment hostEnvironment, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: AddRecipe
        [Authorize(Roles=SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            
            var recipes = _unitOfWork.Recipe.GetAll(includeProperties: "Category");
            return View(await recipes);
        }
        [AllowAnonymous]
        public async Task<IActionResult> MainView(string sortOrder, string currentFilter, string searchString, string category,int? page)
        {
            
            ViewData["ItemsList"] = await _unitOfWork.Category.GetSelected(select: x => x.CategoryName);
            ViewBag.CurrentCategoryFilter = category;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date" : "date_desc";
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            
            var recipes = await _unitOfWork.Recipe.GetFiltered(includeProperties: "Category,Ingrds,Comments",
                searchString: searchString, category: category, sortOrder: sortOrder);
           
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var recipeList =await recipes.ToListAsync();
            
            var recipeSearchViewModel=await _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeSearchViewModel>>(recipeList).ToListAsync();
            foreach(var item in recipeSearchViewModel)
            {
                if (item.Comments.Any())
                {
                    item.AverageRating= (int) (item.Comments.Select(x => x.Rating).Average());
                   
                }
                
            }
            
            return View(recipeSearchViewModel.ToPagedList(pageNumber, pageSize));
        }
        
        public async Task<PartialViewResult> DisplaySteps()
        {
            var step = new StepCreateViewModel();
            
            return PartialView("_StepPartiall2",step);
        }  
        
        public async Task<PartialViewResult> DisplayIngredient()
        {
            var recipeIngredient = new IngredientCreateViewModel();
            return PartialView("_RecipeIngredientPartiall2",recipeIngredient);
        }  
        // GET: AddRecipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var recipe =
                await _unitOfWork.Recipe.GetFirstOrDefault(includeProperties: "Category", filter: m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }
        [AllowAnonymous]
        public async Task<IActionResult> RecipeDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(x=>x.Id==id,includeProperties: "Category,Steps,Ingrds,Comments");
                
           
            var recipeSDetailsViewModel = _mapper.Map<RecipeDetailsViewModel>(recipe);
            if (recipe.Comments.Any())
            {
                recipeSDetailsViewModel.AverageRating =
                    Convert.ToInt32(recipe.Comments.Select(x => x.Rating).Average());
            }

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipeSDetailsViewModel);
        }
       

        // GET: AddRecipe/Create
        
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(),"Id", "CategoryName");
            return View();
        }

        // POST: AddRecipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,RecipeName,PrepTime,RecipeImage,Difficulty,CategoryId,Steps,Ingrds")] RecipeCreateViewModel recipe,IFormFile file)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                string wwwRootPath=_hostEnvironment.WebRootPath;
                if (recipe.RecipeImage != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\recipes");
                    var extension = Path.GetExtension(recipe.RecipeImage.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        await recipe.RecipeImage.CopyToAsync(fileStreams);
                    }

                   
                    recipe.RecipePath = @"\images\recipes\" + fileName + extension;
                    
                }
                var Recipe = _mapper.Map<Recipe>(recipe);
                Recipe.AppUserId = claim.Value;
                //Recipe.RecipeImage=@"\images\recipes\" + fileName + extension;
                _unitOfWork.Recipe.Add(Recipe);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(MainView));
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Id", recipe.CategoryId);
            return View(recipe);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddComment([Bind("Id,Content,Rating,RecipeId")] CommentViewModel commentViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var comment = _mapper.Map<Comment>(commentViewModel);
                comment.AppUserId = claim.Value;
                comment.UserName = User.Identity.Name;
                _unitOfWork.Comment.Add(comment);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(RecipeDetails),new {id=comment.RecipeId});
            }

            return RedirectToAction(nameof(RecipeDetails),new {id=commentViewModel.RecipeId});
           // return View("RecipeDetails", );
        }

        [Authorize(Roles=SD.Role_Admin)]
        
        // GET: AddRecipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(filter: x => x.Id == id);
            
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Id", recipe.CategoryId);
            return View(recipe);
        }

        // POST: AddRecipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=SD.Role_Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeName,PrepTime,RecipeImage,Difficulty,CategoryId")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Recipe.Update(recipe);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Id", recipe.CategoryId);
            return View(recipe);
        }
        [Authorize(Roles=SD.Role_Admin)]
        // GET: AddRecipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(includeProperties: "Category", filter: m => m.Id == id);
            
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: AddRecipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles=SD.Role_Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(filter: m => m.Id == id);
            _unitOfWork.Recipe.Remove(recipe);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _unitOfWork.Recipe.CheckIfExists(x => x.Id == id);
        }
    }
}
