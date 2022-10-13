using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("Asignaciones", Schema = "app")]
    public class Asignacion
    {
        public int Id { get; set; }
        [Required]
        public DateTime Fecha_Inicio { get; set; }
        [Required]
        public DateTime Fecha_Final { get; set; }
        [Required]

        public int IdColaborador { get; set; }
        [ForeignKey("IdColaborador")]
        public Colaborador Colaborador { get; set; }

        public List<Distribucion> Distribuciones { get; set; }
        
        
    }
}
