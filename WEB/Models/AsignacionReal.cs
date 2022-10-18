using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{

        [Table("AsignacionesReal", Schema = "app")]
        public class AsignacionReal
        {
            public int Id { get; set; }
            public int IdAsignacion { get; set; }
            [ForeignKey("IdAsignacion")]
            public Asignacion Asignacion { get; set; }
            [Required]
            public DateTime Fecha_Inicio { get; set; }
            [Required]
            public DateTime Fecha_Final { get; set; }
            public List<DistribucionReal> DistribucionesReales { get; set; }

        }
 }
