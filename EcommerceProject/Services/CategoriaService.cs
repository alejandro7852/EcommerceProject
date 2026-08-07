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

        public async Task AddAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                Nombre = viewModel.Nombre,
            };

            await _categoriaRepo.AddAsync(entity);
        }

        public async Task<CategoriaVM?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepo.GetByIdAsync(id);
            var categoriaVM = new CategoriaVM();

            if(categoria!=null)
            {
                categoriaVM.Nombre = categoria.Nombre;
                categoriaVM .CategoriaId = categoria.CategoriaId;
            }
            return categoriaVM;
        }
        public async Task EditAsync(CategoriaVM viewModel)
        {
            var entity = new Categoria
            {
                CategoriaId = viewModel.CategoriaId,
                Nombre = viewModel.Nombre,
            };
            await _categoriaRepo.EditAsync(entity);
        }
        public async Task EliminarAsync(int id)
        {
            var categoria = await _categoriaRepo.GetByIdAsync(id);
            await _categoriaRepo.DeleteAsync(categoria!);
        }

    }
}
