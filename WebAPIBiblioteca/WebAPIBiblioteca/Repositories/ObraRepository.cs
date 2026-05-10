using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public class ObraRepository : IObraRepository
    {
        public List<Obra> GetAll()
        {
            return DALPro.Query<Obra>("SELECT * FROM Obra");
        }

        public Obra GetById(int id)
        {
            string sql = "SELECT * FROM Obra WHERE ObraID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Obra>(sql, param).FirstOrDefault();
        }

        public int Insert(Obra o, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Obra (Titulo, Autor, ISBN, Capa, TemaID)
                           VALUES (@Titulo, @Autor, @ISBN, @Capa, @TemaID);
                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                { "@Titulo", o.Titulo },
                { "@Autor", o.Autor },
                { "@ISBN", o.ISBN },
                { "@Capa", (object)o.Capa ?? DBNull.Value },
                { "@TemaID", o.TemaID }
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Obra o, SqlTransaction trans)
        {
            string sql = @"UPDATE Obra
                           SET Titulo=@Titulo,
                               Autor=@Autor,
                               ISBN=@ISBN,
                               Capa=@Capa,
                               TemaID=@TemaID
                           WHERE ObraID=@ObraID";

            var param = new Dictionary<string, object>
            {
                { "@ObraID", o.ObraID },
                { "@Titulo", o.Titulo },
                { "@Autor", o.Autor },
                { "@ISBN", o.ISBN },
                { "@Capa", (object)o.Capa ?? DBNull.Value },
                { "@TemaID", o.TemaID }
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Obra WHERE ObraID=@id";
            var param = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, param, trans);
        }
    }
}