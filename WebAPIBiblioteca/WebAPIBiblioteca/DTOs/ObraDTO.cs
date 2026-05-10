namespace WebAPIBiblioteca.DTOs
{
    public class ObraDTO
    {
        public int ObraID { get; set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public string ISBN { get; set; }

        public string Capa { get; set; }

        public int TemaID { get; set; }
    }
}