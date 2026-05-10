using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface IExemplarRequisicaoRepository
    {
        List<ExemplarRequisicao> GetByRequisicao(int requisicaoId);
        List<ExemplarRequisicao> GetByExemplar(int exemplarId);
        ExemplarRequisicao GetByPair(int exemplarId, int requisicaoId);
        void Insert(ExemplarRequisicao er, SqlTransaction trans);
        void MarcarDevolvido(int exemplarId, int requisicaoId, SqlTransaction trans);
        void Delete(int exemplarId, int requisicaoId, SqlTransaction trans);
    }
}