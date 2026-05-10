using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class ExemplarRepository : IExemplarRepository
    {
        public List<Exemplar> GetAll()
        {
            return DALPro.Query<Exemplar>("SELECT * FROM Exemplar");
        }

        public Exemplar GetById(int id)
        {
            string sql = "SELECT * FROM Exemplar WHERE ExemplarID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Exemplar>(sql, param).FirstOrDefault();
        }

        public int Insert(Exemplar e, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Exemplar (ObraID, NucleoID)
                           VALUES (@ObraID, @NucleoID);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@ObraID", e.ObraID },
                { "@NucleoID", e.NucleoID }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Exemplar e, SqlTransaction trans)
        {
            string sql = @"UPDATE Exemplar
                           SET ObraID=@ObraID,
                               NucleoID=@NucleoID
                           WHERE ExemplarID=@ExemplarID";

            var param = new Dictionary<string, object>
            {
                { "@ExemplarID", e.ExemplarID },
                { "@ObraID", e.ObraID },
                { "@NucleoID", e.NucleoID }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Exemplar WHERE ExemplarID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}