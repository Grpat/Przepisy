using AutoMapper;
using Hostele.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Policy;
using Hostele.ViewModels;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Hostele.Controllers;
using Microsoft.AspNetCore.Authorization;
namespace Hostele.Controllers
{
    public class UserReciptController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserReciptController(IWebHostEnvironment hostEnvironment, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.Email);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var a = await _unitOfWork.Recipe.GetAll(filter:x=>x.AppUserId== userId );
            var a = await _unitOfWork.Recipe.GetAll2(x=>x.AppUserId== userId);
            return View(a);
        }

        
        public async Task<IActionResult>  Details(int? id)
        {
            // var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var b = await _unitOfWork.Recipe.GetAll2(x=>x.AppUserId== userId);
            // foreach (var temp in b)
            // {
            //     RedirectToAction("RecipeDetails", "Recipe");
            //     
            // }
            // var a =RedirectToAction("RecipeDetails", "Recipe");
            // return View();
            // var a = RedirectToAction("RecipeDetails", "Recipe", id);
            string url ="https://localhost:7078/"+ "Recipe/RecipeDetails" + "/" + id;
            var url2 = Redirect(url);
            return url2;
            
            RedirectToAction("RecipeDetails", "Recipe",new {id=id});
        }
    }
}
