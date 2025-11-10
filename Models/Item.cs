using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace bobesponja2._0.Models
{
    public class Item
    {
        public enum StatusItem
        {
            Disponível = 0,
            Indisponível = 1,
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
   
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public StatusItem Status { get; set; } = StatusItem.Disponível;


        public string Preco { get; set; }

    }
}
