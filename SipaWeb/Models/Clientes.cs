using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SipaWeb.Models
{
    public class Clientes
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? Identificacion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Edad { get; set; }
        public string? Genero { get; set; }
        public string? Ciudad { get; set; }
        public string? Dias { get; set; }
        public string? Presupuesto { get; set; }
        public string? Adultos { get; set; }
        public string? Ninos { get; set; }
        public string? Clima { get; set; }
        public string[]? InteresesSeleccionados { get; set; }
        public string? Intereses { get; set; }
        public string? Q8 { get; set; }
        public string? Q9 { get; set; }
        public string? Q10 { get; set; }
        public string? Descripcion { get; set; }

    }
}
