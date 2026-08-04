using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Entities
{
    public class Usuario
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

        public ICollection<Orden> Ordenes { get; set; }
    }
}
