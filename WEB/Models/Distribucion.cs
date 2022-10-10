using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class Distribucion
    {
        public int Id { get; set; }
        public Proyecto Proyecto { get; set; }
        public Asignacion Asignacion { get; set; }
        [Required]
        public int Id_Proyecto { get; set; }
        [Required]
        public int Id_Asignacion { get; set; }
        [Required]
        [MaxLength(3)]
        public int Porcentaje { get; set; }
    }
}
