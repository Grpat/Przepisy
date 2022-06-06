using AutoMapper;
using Hostele.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            var a = await _unitOfWork.Recipe.GetAll(filter:x=>x.AppUserId== claim.Value );
            return View(a);
        }
    }
}
