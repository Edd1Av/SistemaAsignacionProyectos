using Microsoft.AspNetCore.Identity;
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
        public string Nombres { get; set; }
        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }
        [Required]
        [StringLength(18)]
        public string CURP { get; set; }
        [Required]
        [StringLength(50)]
        public string Id_Odoo { get; set; }

        public ApplicationUser IdentityUser { get; set; }

    }
}
