using EcommerceProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class CategoriaController(CategoriaService _categoriaService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.GetAllAsync();

            return View(categorias);
        }
    }
}
