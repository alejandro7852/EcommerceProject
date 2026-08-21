using EcommerceProject.Models;
using EcommerceProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class CuentasController(UsuarioService _usuarioService) : Controller
    {
        public IActionResult Login()
        {
            var viewModel = new LoginVM();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM entityVM)
        {

            if (!ModelState.IsValid) return View(entityVM);
            var encontrado = await _usuarioService.Login(entityVM);

            if (encontrado.UsuarioID == 0)
            {
                ViewBag.message = "Usuario no encontrado.";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Registrarse()
        {
            var viewModel = new UsuarioVM();

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM entityVM)
        {

            if (!ModelState.IsValid) return View(entityVM);
            try
            {
                await _usuarioService.Registrase(entityVM);
                ViewBag.message = "Su cuenta ha sido registrada con exito!";
                ViewBag.Class = "alert alert-success";

            }
            catch (Exception ex)
            {
                ViewBag.message = "Error al registrar el usuario: " + ex.Message;
                ViewBag.Class = "alert alert-danger";
                
            }
            return View();
        }
    }
}
