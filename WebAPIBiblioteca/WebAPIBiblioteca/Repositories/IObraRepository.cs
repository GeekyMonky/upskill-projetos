using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface IObraRepository
    {
        List<Obra> GetAll();
        Obra GetById(int id);
        int Insert(Obra obra, SqlTransaction trans);
        void Update(Obra obra, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}