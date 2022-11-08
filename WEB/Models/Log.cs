using Data.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    [Table("Logger", Schema = "app")]
    public class Log
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Id_User { get; set; }
        public DateTime Created { get; set; }
        public string Accion { get; set; }
        public string Description { get; set; }
    }
}
