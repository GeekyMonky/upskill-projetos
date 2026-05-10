using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class TemaRepository : ITemaRepository
    {
        public List<Tema> GetAll()
        {
            return DALPro.Query<Tema>("SELECT * FROM Tema");
        }

        public Tema GetById(int id)
        {
            string sql = "SELECT * FROM Tema WHERE TemaID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Tema>(sql, param).FirstOrDefault();
        }

        public int Insert(Tema t, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Tema (Designacao)
                           VALUES (@Designacao);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@Designacao", t.Designacao }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Tema t, SqlTransaction trans)
        {
            string sql = @"UPDATE Tema
                           SET Designacao=@Designacao
                           WHERE TemaID=@TemaID";

            var param = new Dictionary<string, object>
            {
                { "@TemaID", t.TemaID },
                { "@Designacao", t.Designacao }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Tema WHERE TemaID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}