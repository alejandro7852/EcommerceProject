using EcommerceProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace EcommerceProject.Controllers
{
    public class UserController(OrdenService _ordenService) : Controller
    {
        [Authorize]
        public async Task<IActionResult> MisOrdenes()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var ordenesVM = await _ordenService.GetOrdenesByUserIdAsync(int.Parse(userId));


            return View(ordenesVM);
        }
    }
}
