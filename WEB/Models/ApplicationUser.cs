using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int IdColaborador { get; set; }
        [ForeignKey("IdColaborador")]
        public Colaborador Colaborador { get; set; }

    }
}