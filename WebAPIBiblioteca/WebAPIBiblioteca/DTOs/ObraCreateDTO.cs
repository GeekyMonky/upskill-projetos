namespace WebAPIBiblioteca.DTOs
{
    public class ObraCreateDTO
    {
        public string Titulo { get; set; }

        public string Autor { get; set; }

        public string ISBN { get; set; }

        public string Capa { get; set; }

        public int TemaID { get; set; }
    }
}