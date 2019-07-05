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
        [Required]
        public string DESCRIPCION { get; set; }
        [Required]
        public double PRECIOUNITARIO { get; set; }
        [Required]
        public string CODIGO { get; set; }
        [Required]
        public int STOCK { get; set; }

        public ICollection<Pedidos> Pedidos { get; set; }

    }
}

