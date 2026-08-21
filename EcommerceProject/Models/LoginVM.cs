using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
