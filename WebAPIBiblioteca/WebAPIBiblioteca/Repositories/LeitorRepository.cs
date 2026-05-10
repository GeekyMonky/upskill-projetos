using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class LeitorRepository : ILeitorRepository
    {
        public List<Leitor> GetAll()
        {
            return DALPro.Query<Leitor>("SELECT * FROM Leitor");
        }

        public Leitor GetById(int id)
        {
            string sql = "SELECT * FROM Leitor WHERE LeitorID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Leitor>(sql, param).FirstOrDefault();
        }

        public int Insert(Leitor l, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Leitor (Nome, Email, EstaAtivo)
                           VALUES (@Nome, @Email, @EstaAtivo);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@Nome", l.Nome },
                { "@Email", l.Email },
                { "@EstaAtivo", l.EstaAtivo }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Leitor l, SqlTransaction trans)
        {
            string sql = @"UPDATE Leitor
                           SET Nome=@Nome,
                               Email=@Email,
                               EstaAtivo=@EstaAtivo
                           WHERE LeitorID=@LeitorID";

            var param = new Dictionary<string, object>
            {
                { "@LeitorID", l.LeitorID },
                { "@Nome", l.Nome },
                { "@Email", l.Email },
                { "@EstaAtivo", l.EstaAtivo }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Leitor WHERE LeitorID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}