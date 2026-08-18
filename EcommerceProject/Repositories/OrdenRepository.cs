using EcommerceProject.Context;
using EcommerceProject.Entities;

namespace EcommerceProject.Repositories
{
    public class OrdenRepository : GenericRepository<Orden>
    {
        private readonly AppDbContext _dbContext;
        public OrdenRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task AddAsync(Orden orden)
        {
           using var transaccion = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var detalle in orden.OrdenItems)
                {
                    var producto = await _dbContext.Producto.FindAsync(detalle.ProductoId);
                    producto.Stock -= detalle.Cantidad;


                }
                await _dbContext.Orden.AddAsync(orden);
                await _dbContext.SaveChangesAsync();    

                await transaccion.CommitAsync();

            }
            catch 
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
    }
}
