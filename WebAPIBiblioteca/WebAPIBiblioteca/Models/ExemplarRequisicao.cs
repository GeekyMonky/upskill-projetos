namespace WebAPIBiblioteca.Models
{
    public class ExemplarRequisicao
    {
        public int ExemplarID { get; set; }

        public int RequisicaoID { get; set; }

        public DateTime? DataDevolucao { get; set; }
    }
}