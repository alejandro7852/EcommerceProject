using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Models.Payment;
using EcommerceProject.Services;
using EcommerceProject.Services.Payment;
using EcommerceProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace EcommerceProject.Controllers
{
   
    public class HomeController(
        CategoriaService _categoriaService,
        ProductoService _productoService,
        OrdenService _ordenService,
        PaymentService _paymentService
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
            var catalogo = new CatalogoVM { Categorias = categorias, Productos = productos, filtro = $"Resultados para: {busqueda}" };

            return View("index",catalogo);
        }
        public async Task<IActionResult> ProductoDetalle(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarItemCarrito(int productoId, int cantidad)
        {
            var producto = await _productoService.GetByIdAsync(productoId);


            var carro = HttpContext.Session.Get<List<CarroItemVM>>("Carro") ?? new List<CarroItemVM>();

            if (carro.Find(x => x.ProductoId == productoId) == null)
            {
                carro.Add(new CarroItemVM
                {
                    ProductoId = productoId,
                    Nombre = producto.Nombre,
                    ImagenNombre = producto.NombreImagen ?? producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = cantidad
                });
            }
            else
            {
                var actualizarCarrito = carro.Find(x => x.ProductoId == productoId);
                actualizarCarrito!.Cantidad += cantidad;
            }
            HttpContext.Session.Set("Carro", carro);
            ViewBag.message = "Producto agregado al carrito";

            return View("ProductoDetalle", producto);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult VerCarro()
        {
            var carro = HttpContext.Session.Get<List<CarroItemVM>>("Carro") ?? new List<CarroItemVM>();

            return View(carro);
        }
        public IActionResult EliminarProductoCarro(int productoId)
        {
            var carro = HttpContext.Session.Get<List<CarroItemVM>>("Carro");
            var producto = carro.Find(x => x.ProductoId == productoId);
            carro.Remove(producto!);
            HttpContext.Session.Set("Carro", carro);
            return View("VerCarro", carro);
        }
        [HttpPost]
        //public async Task<IActionResult> PagarAhora()
        //{
        //    var carro = HttpContext.Session.Get<List<CarroItemVM>>("Carro") ;

        //    var userid = 1;
        //    await _ordenService.AddAsync(carro, userid);

        //    HttpContext.Session.Remove("Carro");

        //    return View("VentaCompletada", carro);
        //}
        [HttpPost]
        public async Task<IActionResult> PagarAhora()
        {
            var carro = HttpContext.Session.Get<List<CarroItemVM>>("Carro");

            if (carro == null || !carro.Any())
            {
                return RedirectToAction("Carrito");
            }

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var orden = await _ordenService.AddAsync(carro, int.Parse(userId));

            // Identificador único de la compra para Webpay
            var buyOrder = $"ORDEN-{orden.OrdenId}";

            // Identificador de sesión para Webpay
            var sessionId = Guid.NewGuid().ToString();

            // URL a la que Webpay devolverá al usuario
            var returnUrl = Url.Action(
                "WebpayReturn",
                "Home",
                null,
                Request.Scheme);

            // Crear transacción Webpay
            var transaction = await _paymentService.CreateTransactionAsync(
                buyOrder,
                sessionId,
                orden.TotalOrden,
                returnUrl!);



            var model = new WebpayRedirectVM
            {
                Url = transaction.Url,
                Token = transaction.Token
            };

            return View("RedirectToWebpay", model);
        }
        public async Task<IActionResult> WebpayReturn(string token_ws)
        {
            if (string.IsNullOrEmpty(token_ws))
            {
                return Content("La transacción no contiene un token de Webpay.");
            }

            var response = _paymentService.CommitTransaction(token_ws);

            if (response.ResponseCode == 0)
            {
                var ordenId = int.Parse(
                    response.BuyOrder.Replace("ORDEN-", "")
                );

                var orden = await _ordenService.GetByIdAsync(ordenId);

                if (orden == null)
                {
                    return Content("No se encontró la orden.");
                }

                orden.Estado = EstadoOrden.Pagado;

                await _ordenService.UpdateAsync(orden);

                await _ordenService.DescontarStockAsync(orden);

                HttpContext.Session.Remove("Carro");

                ViewBag.OrdenId = orden.OrdenId;
                ViewBag.Monto = response.Amount;
                ViewBag.Estado = response.Status;
                ViewBag.BuyOrder = response.BuyOrder;
                ViewBag.PagoRechazado = false;

                return View("VentaCompletada");
            }

            ViewBag.OrdenId = int.Parse(
                response.BuyOrder.Replace("ORDEN-", "")
            );

            ViewBag.Monto = response.Amount;
            ViewBag.Estado = response.Status;
            ViewBag.ResponseCode = response.ResponseCode;
            ViewBag.PagoRechazado = true;

            return View("VentaCompletada");
        }
        public IActionResult VentaCompletada()
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
