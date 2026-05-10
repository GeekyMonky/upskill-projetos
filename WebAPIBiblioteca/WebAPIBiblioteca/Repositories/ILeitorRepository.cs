using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface ILeitorRepository
    {
        List<Leitor> GetAll();
        Leitor GetById(int id);
        int Insert(Leitor leitor, SqlTransaction trans);
        void Update(Leitor leitor, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}