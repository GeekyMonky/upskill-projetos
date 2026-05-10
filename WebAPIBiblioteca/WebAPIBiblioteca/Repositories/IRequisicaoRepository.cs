using Microsoft.Data.SqlClient;
using WebAPIBiblioteca.Models;

namespace WebAPIBiblioteca.Repositories
{
    public interface IRequisicaoRepository
    {
        List<Requisicao> GetAll();
        Requisicao GetById(int id);
        int Insert(Requisicao requisicao, SqlTransaction trans);
        void Delete(int id, SqlTransaction trans);
    }
}