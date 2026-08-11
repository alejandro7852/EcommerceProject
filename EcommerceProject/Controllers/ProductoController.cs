using EcommerceProject.Models;
using EcommerceProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class ProductoController(ProductoService _productoService): Controller
    {
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetAllAsync();

            return View(productos);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var productoVM = await _productoService.GetByIdAsync(id);
            return View(productoVM);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductoVM entityVM)
        {
            ViewBag.message = null;
            ModelState.Remove("Categorias");
            ModelState.Remove("Categoria.Nombre");
            if (!ModelState.IsValid) return View(entityVM);

            if (entityVM.ProductoId == 0)
            {
                await _productoService.AddAsync(entityVM);
                ModelState.Clear();
                entityVM = new ProductoVM();
                ViewBag.message = "El Producto ha sido Agregado correctamente.";
            }
            else
            {
                await _productoService.EditAsync(entityVM);
                ViewBag.message = "El Producto ha sido editado correctamente.";
            }


            return View(entityVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.EliminarAsync(id);
            return RedirectToAction("Index");
        }
    }
}
