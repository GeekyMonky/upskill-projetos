using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface IExemplarRepository
    {
        List<Exemplar> GetAll();
        Exemplar GetById(int id);
        int Insert(Exemplar exemplar, SqlTransaction trans);
        void Update(Exemplar exemplar, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}