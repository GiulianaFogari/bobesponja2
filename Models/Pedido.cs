using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace bobesponja2._0.Models
{
    public class Pedido
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PratoId { get; set; }
        public DateTime DataHora { get; set; }
    }
}
