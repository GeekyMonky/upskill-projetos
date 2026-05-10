namespace WebAPIBiblioteca.DTOs
{
    public class RequisicaoDTO
    {
        public int RequisicaoID { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public int LeitorID { get; set; }
    }
}