#nullable disable
using System;
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
using X.PagedList;


namespace Hostele.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public RecipeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: AddRecipe
        [Authorize(Roles=SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            
            var recipes = _unitOfWork.Recipe.GetAll(include: x => x
                .Include(a => a.Category));
            return View(await recipes);
        }
        [AllowAnonymous]
        public async Task<IActionResult> MainView(string sortOrder, string currentFilter, string searchString, string category,int? page)
        {
            ViewData["ItemsList"] = await _context.Categories.Select(x => x.CategoryName).ToListAsync();
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

            //Should be done in reposiotory as Queryable 
            var recipes = from s in await _unitOfWork.Recipe.GetAll(include: x => x
                    .Include(a=>a.Category)
                    .Include(a => a.Ingrds))
                select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.Ingrds != null && (s.RecipeName.ToUpper().Contains(searchString.ToUpper())
                                                                               ||  s.Ingrds.Select(x=>x.IngredientName.ToUpper()).Contains(searchString.ToUpper())));
            }
            if (!String.IsNullOrEmpty(category))
            {
                recipes = recipes.Where(s => s.Category != null && s.Category.CategoryName==category);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    recipes = recipes.OrderByDescending(s => s.RecipeName);
                    break;
                case "Date":
                    recipes = recipes.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    recipes = recipes.OrderByDescending(s => s.CreatedDate);
                    break;
                default:  // Name ascending 
                    recipes = recipes.OrderBy(s => s.RecipeName);
                    break;
            } 
           
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var recipeList =await recipes.ToListAsync();
            
            var recipeSearchViewModel=_mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeSearchViewModel>>(recipeList);

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

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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

            var recipe = await _unitOfWork.Recipe.GetFirstOrDefault(x => x.Id == id, include: y => y
                .Include(r => r.Category)
                .Include(r => r.Steps)
                .Include(r => r.Ingrds));
                
           
            var recipeSDetailsViewModel = _mapper.Map<RecipeDetailsViewModel>(recipe);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipeSDetailsViewModel);
        }
       

        // GET: AddRecipe/Create
        
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
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
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\recipes");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStreams);
                    }

                   
                    recipe.RecipeImage = @"\images\recipes\" + fileName + extension;
                    
                }
                var Recipe = _mapper.Map<Recipe>(recipe);
                Recipe.AppUserId = claim.Value;
                _context.Add(Recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            return View(recipe);
        }
        [Authorize(Roles=SD.Role_Admin)]
        // GET: AddRecipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
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
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
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

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
