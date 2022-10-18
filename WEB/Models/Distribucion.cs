using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("DistribucionAsignacion", Schema = "app")]
    public class Distribucion
    {
        public int Id { get; set; }
        [Required]
        public int IdAsignacion { get; set; }
        [ForeignKey("IdAsignacion")]
        public virtual Asignacion Asignacion { get; set; }
        [Required]
        public int IdProyecto { get; set; }
        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }
        [Required]
        public DateTime Fecha_Inicio { get; set; }
        [Required]
        public DateTime Fecha_Final { get; set; }
        //[Required]
        //[MaxLength(3)]
        //public int Porcentaje { get; set; }
    }
}
