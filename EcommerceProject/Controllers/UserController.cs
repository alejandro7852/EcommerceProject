using EcommerceProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class UserController(OrdenService _ordenService) : Controller
    {
        public async Task<IActionResult> MisOrdenes()
        {
            //TODO : change id
            var userId = 1;
            var ordenesVM = await _ordenService.GetOrdenesByUserIdAsync(userId);


            return View(ordenesVM);
        }
    }
}
