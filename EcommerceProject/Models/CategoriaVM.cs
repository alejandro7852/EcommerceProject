using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class CategoriaVM
    {
        public int CategoriaId { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}
