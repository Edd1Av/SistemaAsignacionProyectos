using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("Colaboradores", Schema = "app")]
    public class Colaborador
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public String Nombres { get; set; }
        [Required]
        [StringLength(100)]
        public String Apellidos { get; set; }
        [Required]
        [StringLength(18)]
        public String CURP { get; set; }
        [Required]
        [StringLength(13)]
        public String RFC { get; set; }
        [Required]
        [StringLength(50)]
        public String Id_Odoo { get; set; }
    }
}
