using Data.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("Logger", Schema = "app")]
    public class Log
    {
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Accion { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
