using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class NucleoRepository : INucleoRepository
    {
        public List<Nucleo> GetAll()
        {
            return DALPro.Query<Nucleo>("SELECT * FROM Nucleo");
        }

        public Nucleo GetById(int id)
        {
            string sql = "SELECT * FROM Nucleo WHERE NucleoID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Nucleo>(sql, param).FirstOrDefault();
        }

        public int Insert(Nucleo n, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Nucleo (Distrito, EhCentral)
                           VALUES (@Distrito, @EhCentral);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@Distrito", n.Distrito },
                { "@EhCentral", n.EhCentral }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Nucleo n, SqlTransaction trans)
        {
            string sql = @"UPDATE Nucleo
                           SET Distrito=@Distrito,
                               EhCentral=@EhCentral
                           WHERE NucleoID=@NucleoID";

            var param = new Dictionary<string, object>
            {
                { "@NucleoID", n.NucleoID },
                { "@Distrito", n.Distrito },
                { "@EhCentral", n.EhCentral }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Nucleo WHERE NucleoID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}