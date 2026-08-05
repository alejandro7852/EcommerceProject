using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Services
{
    public class CategoriaService(GenericRepository<Categoria> _categoriaRepo)
    {
        public async Task<IEnumerable<CategoriaVM>> GetAllAsync()
        {
           var categorias = await _categoriaRepo.GetAllAsync();

            var categoriasVM = categorias.Select(item =>
            new CategoriaVM
            {
                CategoriaId = item.CategoriaId,
                Nombre = item.Nombre,
            }).ToList();
            return categoriasVM;
        }
    }
}
