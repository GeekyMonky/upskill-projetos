namespace WebAPIBiblioteca.Models
{
    public class Leitor
    {
        public int LeitorID { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public bool EstaAtivo { get; set; }

        public int NumeroAtrasos { get; set; }

        public DateTime? DataUltimaRequisicao { get; set; }

        public DateTime DataRegisto { get; set; }
    }
}