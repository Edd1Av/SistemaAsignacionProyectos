using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("DistribucionAsignacionReal", Schema = "app")]
    public class DistribucionReal
    {
        public int Id { get; set; }
        [Required]
        public int IdAsignacionReal { get; set; }
        [ForeignKey("IdAsignacionReal")]
        public AsignacionReal AsignacionReal { get; set; }
        [Required]
        public int IdProyecto { get; set; }
        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }
        [Required]
        [MaxLength(3)]
        public int Porcentaje { get; set; }
    }
}
