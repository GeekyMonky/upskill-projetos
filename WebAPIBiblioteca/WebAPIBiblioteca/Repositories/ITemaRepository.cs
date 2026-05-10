using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface ITemaRepository
    {
        List<Tema> GetAll();
        Tema GetById(int id);
        int Insert(Tema tema, SqlTransaction trans);
        void Update(Tema tema, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}