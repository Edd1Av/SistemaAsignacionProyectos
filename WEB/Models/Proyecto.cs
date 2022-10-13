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
        public string Clave { get; set; }
        [StringLength(200)]
        [Required]
        public string Titulo { get; set; }
        
    }
}
