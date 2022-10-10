using System.ComponentModel.DataAnnotations;

namespace WEB.Models
{
    public class Asignacion
    {
        public int Id { get; set; }
        [Required]
        public DateTime Fecha_Inicio { get; set; }
        [Required]
        public DateTime Fecha_Final { get; set; }
        public Colaborador Colaborador { get; set; }
        public List<Distribucion> Distribuciones { get; set; }
        [Required]
        public int Id_Colaborador { get; set; }
    }
}
