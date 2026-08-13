namespace EcommerceProject.Models
{
    public class CatalogoVM
    {
        public IEnumerable<CategoriaVM> Categorias { get; set; }
        public IEnumerable<ProductoVM> Productos { get; set; }
        public String filtro { get; set; }
    }
}
