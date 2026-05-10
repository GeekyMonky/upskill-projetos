using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface INucleoRepository
    {
        List<Nucleo> GetAll();
        Nucleo GetById(int id);
        int Insert(Nucleo nucleo, SqlTransaction trans);
        void Update(Nucleo nucleo, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}