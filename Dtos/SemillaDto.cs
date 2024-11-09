namespace Sowing_O2.Dtos
{
    
    public class CreateSemillaDto
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int IdCategoria { get; set; }
        public string Ubicacion { get; set; }
    }


    public class SemillaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int IdCategoria { get; set; }
        public string Ubicacion { get; set; }
    }
    public class TrasladoDto
    {
        public string NuevaUbicacion { get; set; }
        public int IdUsuario { get; set; }
    }

}
