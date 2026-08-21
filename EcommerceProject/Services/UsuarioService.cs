using EcommerceProject.Entities;
using EcommerceProject.Models;
using EcommerceProject.Repositories;
using System.Linq.Expressions;

namespace EcommerceProject.Services
{
    public class UsuarioService(GenericRepository<Usuario> usuarioRepository)
    {
        public async Task<UsuarioVM> Login(LoginVM loginVM)
        {
            var condicion = new List<Expression<Func<Usuario, bool>>>()
            {
                u => u.Email == loginVM.Email,
                u => u.Clave == loginVM.Clave
            };

            var encontrado = await usuarioRepository.GetByFiltroAsync(conditions: condicion.ToArray());

            var usuarioVM = new UsuarioVM();
            if (encontrado != null)
            {
                usuarioVM.UsuarioID = encontrado.UsuarioID;
                usuarioVM.Nombre = encontrado.Nombre;
                usuarioVM.Email = encontrado.Email;
                usuarioVM.Tipo = encontrado.Tipo;   

            }
            return usuarioVM;
        }

        public async Task Registrase(UsuarioVM usuarioVM)
        {
            if (usuarioVM.Clave != usuarioVM.RepetirClave)
            {
                throw new InvalidOperationException("Las contraseñas no coinciden.");
            }
            var condicion = new List<Expression<Func<Usuario, bool>>>()
            {
                u => u.Email == usuarioVM.Email
            };

            var Emailencontrado = await usuarioRepository.GetByFiltroAsync(conditions: condicion.ToArray());
            if (Emailencontrado != null)
            {
                throw new InvalidOperationException("El correo electrónico ya está registrado.");
            }

            var entidad = new Usuario
            {
                Nombre = usuarioVM.Nombre,
                Email = usuarioVM.Email,
                Clave = usuarioVM.Clave,
                Tipo = usuarioVM.Tipo
            };
            await usuarioRepository.AddAsync(entidad);
        }
    }
}
