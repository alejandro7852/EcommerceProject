using EcommerceProject.Entities;

namespace EcommerceProject.Models
{
    public class OrdenVM
    {
        public DateTime OrdenFecha { get; set; }
        public decimal TotalOrden { get; set; }
        public ICollection<OrdenItemVM>? OrdenItems { get; set; }
        public EstadoOrden Estado { get; set; }
    }
}
