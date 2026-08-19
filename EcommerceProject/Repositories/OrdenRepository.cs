using EcommerceProject.Context;
using EcommerceProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {
        private readonly AppDbContext _dbContext;
        public OrdenRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task<Orden> AddAsync(Orden orden)
        {
           using var transaccion = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                //foreach (var detalle in orden.OrdenItems)
                //{
                //    var producto = await _dbContext.Producto.FindAsync(detalle.ProductoId);
                //    producto.Stock -= detalle.Cantidad;


                //}
                await _dbContext.Orden.AddAsync(orden);
                await _dbContext.SaveChangesAsync();    

                await transaccion.CommitAsync();

            }
            catch 
            {
                await transaccion.RollbackAsync();
                throw;
            }
            return orden;
        }
        public async Task<Orden?> GetByIdWithItemsAsync(int ordenId)
        {
            return await _dbContext.Orden
                .Include(x => x.OrdenItems)
                .FirstOrDefaultAsync(x => x.OrdenId == ordenId);
        }
        public async Task DescontarStockAsync(Orden orden)
        {
            foreach (var detalle in orden.OrdenItems)
            {
                var producto = await _dbContext.Producto.FindAsync(detalle.ProductoId);

                if (producto == null)
                {
                    throw new Exception($"No se encontró el producto {detalle.ProductoId}");
                }

                producto.Stock -= detalle.Cantidad;
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Orden>> GetOrdenesByUserIdAsync(int userId)
        {
            var ordenes = await _dbContext.Orden
                .Where(o => o.UsuarioId == userId)
                .Include(o => o.OrdenItems)
                .ThenInclude(x => x.Producto).ToListAsync();

            return ordenes;
        }
    }
}
