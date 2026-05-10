using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class RequisicaoRepository : IRequisicaoRepository
    {
        public List<Requisicao> GetAll()
        {
            return DALPro.Query<Requisicao>("SELECT * FROM Requisicao");
        }

        public Requisicao GetById(int id)
        {
            string sql = "SELECT * FROM Requisicao WHERE RequisicaoID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Requisicao>(sql, param).FirstOrDefault();
        }

        public int Insert(Requisicao r, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Requisicao (LeitorID)
                           VALUES (@LeitorID);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@LeitorID", r.LeitorID }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Requisicao WHERE RequisicaoID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}