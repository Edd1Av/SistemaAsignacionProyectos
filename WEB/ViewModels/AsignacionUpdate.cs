namespace WEB.ViewModels
{
    public class AsignacionUpdate
    {
        public int Id { get; set; }
        public int Id_Colaborador { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Final { get; set; }
        public List<ProyectosAsignados> Proyectos { get; set; }
    }
}
