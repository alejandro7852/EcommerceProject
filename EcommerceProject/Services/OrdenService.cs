using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Repositories;

namespace EcommerceProject.Services
{
    public class OrdenService(OrdenRepository _ordenRepository)
    {
        //public async Task AddAsync(List<CarroItemVM> carroItemVM, int userId)
        //{
        //    Orden orden = new Orden()
        //    {
        //        OrdenFecha = DateTime.Now,
        //        UsuarioId = userId,
        //        Estado = EstadoOrden.Pendiente,
        //        TotalOrden = carroItemVM.Sum(x => x.Precio * x.Cantidad),
        //        OrdenItems = carroItemVM.Select(x => new OrdenItem
        //        {
        //            ProductoId = x.ProductoId,
        //            Cantidad = x.Cantidad,
        //            Precio = x.Precio,

        //        }).ToList()
        //    };
        //    await _ordenRepository.AddAsync(orden); 
        //}
        public async Task<Orden> AddAsync(List<CarroItemVM> carroItemVM, int userId)
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
                    Precio = x.Precio
                }).ToList()
            };

            return await _ordenRepository.AddAsync(orden);
        }
        public async Task DescontarStockAsync(Orden orden)
        {

            await _ordenRepository.DescontarStockAsync(orden);
        }
        //public async Task<Orden?> GetByIdAsync(int ordenId)
        //{
        //    return await _ordenRepository.GetByIdAsync(ordenId);
        //}
        public async Task<Orden?> GetByIdAsync(int ordenId)
        {
            return await _ordenRepository.GetByIdWithItemsAsync(ordenId);
        }
        public async Task UpdateAsync(Orden orden)
        {
            await _ordenRepository.EditAsync(orden);
        }
        public async Task<List<OrdenVM>> GetOrdenesByUserIdAsync(int userId)
        {
            var ordenes = await _ordenRepository.GetOrdenesByUserIdAsync(userId);
            var ordenesVM = ordenes.Select(o => new OrdenVM
            {
                OrdenFecha = o.OrdenFecha,
                TotalOrden = o.TotalOrden,
                Estado = o.Estado,
                OrdenItems = o.OrdenItems?.Select(oi => new OrdenItemVM
                {
                    NombreProducto = oi.Producto.Nombre,
                    Cantidad = oi.Cantidad,
                    Precio = oi.Precio
                }).ToList()
            }).ToList();

            return ordenesVM;
        }
    }
}
