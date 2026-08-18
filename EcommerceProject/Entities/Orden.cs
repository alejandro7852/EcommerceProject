namespace EcommerceProject.Entities
{
    public class Orden
    {
        public int OrdenId { get; set; }

        public DateTime OrdenFecha { get; set; }
        public int UsuarioId { get; set; }
        public decimal TotalOrden { get; set; }
       
        public Usuario? Usuario { get; set; }
        public EstadoOrden Estado { get; set; }
        public ICollection<OrdenItem> OrdenItems { get; set; }
    }
}
