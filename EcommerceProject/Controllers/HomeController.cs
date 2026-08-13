using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceProject.Controllers
{
    public class HomeController(
        CategoriaService _categoriaService,
        ProductoService _productoService
        ) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync();
            var catalogo = new CatalogoVM { Categorias = categorias, Productos = productos };

            return View(catalogo);
        }
        public async Task<IActionResult> FiltrarPorCategoria(int id, string nombre)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync(idCategoria:id); 
            var catalogo = new CatalogoVM { Categorias = categorias, Productos = productos, filtro = nombre };

            return View("index",catalogo);
        }
        [HttpPost]
        public async Task<IActionResult> FiltrarPorBusqueda(string busqueda)
        {
            var categorias = await _categoriaService.GetAllAsync();
            var productos = await _productoService.GetCatalogoAsync(busqueda: busqueda); 
            var catalogo = new CatalogoVM { Categorias = categorias, Productos = productos, filtro = $"Result for:{busqueda}" };

            return View("index",catalogo);
        }
        public async Task<IActionResult> ProductoDetalle(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            return View(producto);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
