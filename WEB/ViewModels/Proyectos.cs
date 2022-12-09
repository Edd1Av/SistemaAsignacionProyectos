namespace WEB.ViewModels
{
    public class ProyectosGet
    {
        List<ProyectosR> proyectos { get; set; }
    }
    public class ProyectosR
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string clave { get; set; }
        public List<ColaboradoresAsignados>? ColaboradoresAsignados { get; set; }
    }

    public class ColaboradoresAsignados
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string ClaveOdoo { get; set; }

    }
}
