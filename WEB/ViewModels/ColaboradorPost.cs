namespace WEB.ViewModels
{
    public class ColaboradorPost
    {
        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }
   
        public string CURP { get; set; }
        public string Id_Odoo { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public List<ProyectosPost>? Proyectos { get; set; }
    }

    public class ProyectosPost 
    { 
        public string Titulo { get; set; }
        public string Clave { get; set; }
    }

}
