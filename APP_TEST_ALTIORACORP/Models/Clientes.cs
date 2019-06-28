using APP_TEST_ALTIORACORP.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace APP_TEST_ALTIORACORP.Models
{
    public class Clientes
    {
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^[0-9]{10,10}$",
         ErrorMessage = "Sólo se aceptan números.")]
        [Key]
        public string IDENTIFICACION { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Sólo se aceptan caracteres.")]
        public string NOMBRES { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Sólo se aceptan caracteres.")]
        public string APELLIDOS { get; set; }

        [Required]
        public string DIRECCION { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 7)]
        [RegularExpression(@"^[0-9]{6,10}$",
            ErrorMessage = "Sólo se aceptan números.")]
        public string TELEFONO { get; set; }

        public ICollection<Pedidos> Pedidos { get; set; }

    }

}

