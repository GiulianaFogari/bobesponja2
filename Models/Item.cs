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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
   
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }

        public string Preco { get; set; }

    }
}
