using EcommerceProject.Models;
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
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var categoriaVM = await _categoriaService.GetByIdAsync(id);
            return View(categoriaVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoriaVM entityVM)
        {
            ViewBag.message = null;

            if (!ModelState.IsValid) return View(entityVM);

            if (entityVM.CategoriaId == 0)
            {
                await _categoriaService.AddAsync(entityVM);
                ModelState.Clear();
                entityVM = new CategoriaVM();
                ViewBag.message = "La categoría ha sido editada correctamente.";
            }
            else
            {
                await _categoriaService.EditAsync(entityVM);
                ViewBag.message = "La categoría ha sido editada correctamente.";
            }


                return View(entityVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.EliminarAsync(id);
            return RedirectToAction("Index");
        }
    }
}
