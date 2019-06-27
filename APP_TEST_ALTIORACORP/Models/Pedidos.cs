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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public string CLIENTE { get; set; }
        public int IDPRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public DateTime FECHAPEDIDO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double PRECIOUNITARIO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TOTAL { get; set; }

        public Clientes Clientes { get; set; }
        public Productos Productos { get; set; }
    }
}
