namespace WebAPIBiblioteca.Models
{
    public class Requisicao
    {
        public int RequisicaoID { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public int LeitorID { get; set; }
    }
}