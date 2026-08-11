using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace EcommerceProject.Services
{
    public class ProductoService(
        GenericRepository<Categoria> _categoriaRepo,
        GenericRepository<Producto> _productoRepo,
        IWebHostEnvironment _webHostEnvironment
            )
    {
        public async Task<IEnumerable<ProductoVM>> GetAllAsync()
        {
            var productos = await _productoRepo.GetAllAsync(
                includes : new Expression<Func<Producto, object>>[] {x=> x.Categoria!}
                );

            var productosVM = productos.Select(item =>
            new ProductoVM
            {
                ProductoId = item.ProductoId,
                Categoria = new CategoriaVM
                {
                    CategoriaId = item.Categoria!.CategoriaId,
                    Nombre = item.Categoria!.Nombre
                },
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                Precio = item.Precio,
                Stock = item.Stock,
                NombreImagen = item.NombreImagen,
            }
            ).ToList();

            return productosVM;
        }

        public async Task<ProductoVM>GetByIdAsync(int id)
        {

            var producto = await _productoRepo.GetByIdAsync(id);
            var categorias = await _categoriaRepo.GetAllAsync();

            var ProductoVM = new ProductoVM();

            if (producto != null)
            {
                ProductoVM = new ProductoVM
                {
                    ProductoId = producto.ProductoId,
                    Categoria = new CategoriaVM
                    {
                        CategoriaId = producto.Categoria!.CategoriaId,
                        Nombre = producto.Categoria!.Nombre
                    },
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    NombreImagen = producto.NombreImagen,
                };
            }
            ProductoVM.Categorias = categorias.Select(item => new SelectListItem
            {
                Value = item.CategoriaId.ToString(),
                Text = item.Nombre.ToString(),
            }).ToList();

            return ProductoVM;
        }
        public async Task AddAsync(ProductoVM viewModel)
        {
            if(viewModel.Imagen != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Imagen.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileString = new FileStream(filePath, FileMode.Create))
                    await viewModel.Imagen.CopyToAsync(fileString);

                viewModel.NombreImagen = uniqueFileName;
            }

            var entity = new Producto
            {
                CategoriaId = viewModel.Categoria.CategoriaId,
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Precio  = viewModel.Precio,
                Stock = viewModel.Stock,    
                NombreImagen = viewModel.NombreImagen,
            };

            await _productoRepo.AddAsync(entity);
        }
        public async Task EditAsync(ProductoVM viewModel)
        {
            var producto = await _productoRepo.GetByIdAsync(viewModel.ProductoId);

            if(viewModel.Imagen != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Imagen.FileName);
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileString = new FileStream(filePath, FileMode.Create))
                    await viewModel.Imagen.CopyToAsync(fileString);

                if (!producto.NombreImagen.IsNullOrEmpty())
                {
                    var previusImage = producto.NombreImagen;
                    string deleteFilePath = Path.Combine(uploadFolder, previusImage);

                    if (File.Exists(deleteFilePath))
                        File.Delete(deleteFilePath);

                }
                viewModel.NombreImagen = uniqueFileName;
            }
            else
            {
                viewModel.NombreImagen = producto.NombreImagen;
            }
            producto.CategoriaId = viewModel.Categoria.CategoriaId;
            producto.Nombre = viewModel.Nombre;
            producto.Descripcion = viewModel.Descripcion;
            producto.Precio = viewModel.Precio;
            producto.Stock = viewModel.Stock;
            producto.NombreImagen = viewModel.NombreImagen;

            await _productoRepo.EditAsync(producto);
        }
        public async Task EliminarAsync(int id)
        {
            var producto = await _productoRepo.GetByIdAsync(id);
            await _productoRepo.DeleteAsync(producto);
        }
    }
}
