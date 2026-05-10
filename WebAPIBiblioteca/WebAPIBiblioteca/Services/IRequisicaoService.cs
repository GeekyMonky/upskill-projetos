using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface IRequisicaoService
    {
        List<RequisicaoDTO> GetAll();
        RequisicaoDTO GetById(int id);
        int Create(RequisicaoCreateDTO dto);
        void Delete(int id);
    }
}