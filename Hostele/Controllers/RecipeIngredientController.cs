#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hostele.Data;
using Hostele.Models;
using Hostele.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Hostele.Controllers
{
    [Authorize(Roles=SD.Role_Admin)]
    public class RecipeIngredientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeIngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipeIngredient
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecipeIngredients.Include(r => r.Ingredients).Include(r => r.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipeIngredient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredients)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return View(recipeIngredient);
        }
        
        // GET: RecipeIngredient/Create
        public IActionResult Create()
        {
            ViewData["IngredientsId"] = new SelectList(_context.Ingredients, "Id", "Id");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            
            return View();
        }

        // POST: RecipeIngredient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipeId,IngredientsId,Amount,Measure")] RecipeIngredient recipeIngredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientsId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientsId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // GET: RecipeIngredient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientsId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientsId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // POST: RecipeIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecipeId,IngredientsId,Amount,Measure")] RecipeIngredient recipeIngredient)
        {
            if (id != recipeIngredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeIngredientExists(recipeIngredient.Id))
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
            ViewData["IngredientsId"] = new SelectList(_context.Ingredients, "Id", "Id", recipeIngredient.IngredientsId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // GET: RecipeIngredient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredients)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return View(recipeIngredient);
        }

        // POST: RecipeIngredient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            _context.RecipeIngredients.Remove(recipeIngredient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeIngredientExists(int id)
        {
            return _context.RecipeIngredients.Any(e => e.Id == id);
        }
    }
}
