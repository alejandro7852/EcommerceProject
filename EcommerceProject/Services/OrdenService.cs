using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Repositories;

namespace EcommerceProject.Services
{
    public class OrdenService(OrdenRepository _ordenRepository)
    {
        public async Task AddAsync(List<CarroItemVM> carroItemVM, int userId)
        {
            Orden orden = new Orden()
            {
                OrdenFecha = DateTime.Now,
                UsuarioId = userId,
                Estado = EstadoOrden.Pendiente,
                TotalOrden = carroItemVM.Sum(x => x.Precio * x.Cantidad),
                OrdenItems = carroItemVM.Select(x => new OrdenItem
                {
                    ProductoId = x.ProductoId,
                    Cantidad = x.Cantidad,
                    Precio = x.Precio,

                }).ToList()
            };
            await _ordenRepository.AddAsync(orden); 
        }
    }
}
