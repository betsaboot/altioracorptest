using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APP_TEST_ALTIORACORP.Models
{
    public class Clientes
    {
        [Key] public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO { get; set; }
        public string TIPO { get; set; }

        public ICollection<Pedidos> Pedidos { get; set; }
    }
}

