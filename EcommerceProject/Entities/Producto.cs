using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Entities
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? NombreImagen { get; set; } = null;

        public Categoria? categoria { get; set; }

    }
}
