using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APP_TEST_ALTIORACORP.Models
{
    public class Pedidos
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("IDENTIFICACION")]
        public string CLIENTE { get; set; }
        [ForeignKey("ID")]
        public int PRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public DateTime FECHAPEDIDO { get; set; }
        public decimal PRECIOUNITARIO { get; set; }
        public decimal TOTAL { get; set; }

        public Clientes Clientes { get; set; }
        public Productos Productos { get; set; }
    }
}
