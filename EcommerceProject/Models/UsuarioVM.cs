using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class UsuarioVM
    {
        public int UsuarioID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string RepetirClave { get; set; }
    }
}
