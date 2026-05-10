using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class ExemplarRequisicaoRepository : IExemplarRequisicaoRepository
    {
        public List<ExemplarRequisicao> GetByRequisicao(int requisicaoId)
        {
            string sql = "SELECT * FROM ExemplarRequisicao WHERE RequisicaoID=@id";
            var param = new Dictionary<string, object> { { "@id", requisicaoId } };
            return DALPro.Query<ExemplarRequisicao>(sql, param);
        }

        public List<ExemplarRequisicao> GetByExemplar(int exemplarId)
        {
            string sql = "SELECT * FROM ExemplarRequisicao WHERE ExemplarID=@id";
            var param = new Dictionary<string, object> { { "@id", exemplarId } };
            return DALPro.Query<ExemplarRequisicao>(sql, param);
        }

        public ExemplarRequisicao GetByPair(int exemplarId, int requisicaoId)
        {
            string sql = @"SELECT * FROM ExemplarRequisicao
                           WHERE ExemplarID=@exemplarId AND RequisicaoID=@requisicaoId";

            var param = new Dictionary<string, object>
            {
                { "@exemplarId", exemplarId },
                { "@requisicaoId", requisicaoId }
            };

            return DALPro.Query<ExemplarRequisicao>(sql, param).FirstOrDefault();
        }

        public void Insert(ExemplarRequisicao er, SqlTransaction trans)
        {
            string sql = @"INSERT INTO ExemplarRequisicao (ExemplarID, RequisicaoID, DataDevolucao)
                           VALUES (@ExemplarID, @RequisicaoID, @DataDevolucao)";

            var param = new Dictionary<string, object>
            {
                { "@ExemplarID", er.ExemplarID },
                { "@RequisicaoID", er.RequisicaoID },
                { "@DataDevolucao", (object)er.DataDevolucao ?? DBNull.Value }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void MarcarDevolvido(int exemplarId, int requisicaoId, SqlTransaction trans)
        {
            string sql = @"UPDATE ExemplarRequisicao
                           SET DataDevolucao=GETDATE()
                           WHERE ExemplarID=@exemplarId AND RequisicaoID=@requisicaoId";

            var param = new Dictionary<string, object>
            {
                { "@exemplarId", exemplarId },
                { "@requisicaoId", requisicaoId }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int exemplarId, int requisicaoId, SqlTransaction trans)
        {
            string sql = @"DELETE FROM ExemplarRequisicao
                           WHERE ExemplarID=@exemplarId AND RequisicaoID=@requisicaoId";

            var param = new Dictionary<string, object>
            {
                { "@exemplarId", exemplarId },
                { "@requisicaoId", requisicaoId }
            };

            DALPro.Execute(sql, param, trans);
        }
    }
}