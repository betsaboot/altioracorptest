using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP_TEST_ALTIORACORP.Models
{
    public class Productos
    {
        [Key]public int ID { get; set; }
        public string DESCRIPCION { get; set; }
        public decimal PRECIOUNITARIO { get; set; }
        public string CODIGO { get; set; }

        public ICollection<Pedidos> Pedidos { get; set; }
    }
}

