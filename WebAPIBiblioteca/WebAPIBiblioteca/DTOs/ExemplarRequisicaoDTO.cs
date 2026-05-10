namespace WebAPIBiblioteca.DTOs
{
    public class ExemplarRequisicaoDTO
    {
        public int ExemplarID { get; set; }

        public int RequisicaoID { get; set; }

        public DateTime? DataDevolucao { get; set; }
    }
}