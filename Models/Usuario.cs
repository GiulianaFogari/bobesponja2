using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace bobesponja2._0.Models
{
     public class Usuario
    {

        public enum TipoUsuario
        {
            Administrador = 0,
            Cliente = 1
        }


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }

        public TipoUsuario Tipo { get; set; } = TipoUsuario.Cliente;

    }
}
