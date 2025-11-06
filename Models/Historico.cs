using System;
using SQLite;

namespace bobesponja2._0.Models
{
    public class Historico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public int UsuarioId { get; set; }
        public int ItemId { get; set; }
        public DateTime DataHora { get; set; }
        
        // Propriedades extras para facilitar a exibição
        public string NomeUsuario { get; set; }
        public string NomeItem { get; set; }
    }
}