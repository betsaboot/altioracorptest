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
        [Required]
        public string CLIENTE { get; set; }
        [Required]
        public int IDPRODUCTO { get; set; }
        [Required]
        public int CANTIDAD { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FECHAPEDIDO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double PRECIOUNITARIO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TOTAL { get; set; }

        public Clientes Clientes { get; set; }
        public Productos Productos { get; set; }
    }
}
