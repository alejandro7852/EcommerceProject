using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class ProductoVM
    {
        public int ProductoId { get; set; }
        public CategoriaVM Categoria { get; set; }
        public List<SelectListItem> Categorias { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? NombreImagen { get; set; } = null;

        public IFormFile? Imagen { get; set; }
    }
}
