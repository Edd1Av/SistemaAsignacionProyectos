using WEB.Models;

namespace WEB.ViewModels
{
    public class AsignacionHistorico
    {
        public int Id { get; set; }
        public Colaborador Colaborador { get; set; }
        public List<Distribucion> Distribucion { get; set; }
        public string Proyectos { get; set; }
    }
}
