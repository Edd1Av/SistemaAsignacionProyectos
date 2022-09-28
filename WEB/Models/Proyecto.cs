using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("Proyectos", Schema = "app")]
    public class Proyecto
    {
        public int Id { get; set; }
        [StringLength(5)]
        [Required]
        public String Clave { get; set; }
        [StringLength(200)]
        [Required]
        public String Titulo { get; set; }
    }
}
