using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Entities
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }

    }
}
