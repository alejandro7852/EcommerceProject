using Microsoft.EntityFrameworkCore;
using EcommerceProject.Entities;

namespace EcommerceProject.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Orden> Orden { get; set; }
        public DbSet<OrdenItem> OrdenItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>(e =>
            {
                e.HasKey("CategoriaId");
                e.Property("CategoriaId").ValueGeneratedOnAdd();
                e.HasData(
                    new Categoria { CategoriaId = 1, Nombre = "Tecnología" },
                    new Categoria { CategoriaId = 2, Nombre = "Habitación" }
                    );

            });
            modelBuilder.Entity<Producto>(e =>
            {
                e.HasKey("ProductoId");
                e.Property("ProductoId").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Categoria).WithMany(p => p.Productos).HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Usuario>(e =>
            {
                e.HasKey("UsuarioID");
                e.Property("UsuarioID").ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Orden>(e =>
            {
                e.HasKey("OrdenId");
                e.Property("OrdenId").ValueGeneratedOnAdd();
                e.Property("TotalOrden").HasColumnType("decimal(10,2)");
                e.Property(e => e.Estado).HasConversion<int>().HasDefaultValue(EstadoOrden.Pendiente);
                e.HasOne(e => e.Usuario).WithMany(p => p.Ordenes).HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<OrdenItem>(e =>
            {
                e.HasKey("Ordenltemld");
                e.Property("Ordenltemld").ValueGeneratedOnAdd();
                e.Property("Precio").HasColumnType("decimal(10,2)");
                e.HasOne(e => e.Orden).WithMany(p => p.OrdenItems).HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(e => e.Producto).WithMany().HasForeignKey(e => e.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
