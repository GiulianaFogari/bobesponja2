using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bobesponja2._0.Models.Usuario;

namespace bobesponja2._0.Models
{
    public sealed class UsuarioAtual
    {
        private static UsuarioAtual instance = new UsuarioAtual();
        private UsuarioAtual() { }

        public static UsuarioAtual Instance => instance;

        public Usuario Usuario { get; private set; }

        public void SetUsuario(Usuario u) => Usuario = u;
        public void Logout() => Usuario = null;
        public bool IsAdmin => Usuario != null && Usuario.Tipo == TipoUsuario.Administrador;
    }
}
