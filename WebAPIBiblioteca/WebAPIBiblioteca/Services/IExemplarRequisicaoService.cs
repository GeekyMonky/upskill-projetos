using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface IExemplarRequisicaoService
    {
        List<ExemplarRequisicaoDTO> GetByRequisicao(int requisicaoId);
        List<ExemplarRequisicaoDTO> GetByExemplar(int exemplarId);
        ExemplarRequisicaoDTO GetByPair(int exemplarId, int requisicaoId);
        void Adicionar(int requisicaoId, ExemplarRequisicaoCreateDTO dto);
        void MarcarDevolvido(int exemplarId, int requisicaoId);
        void Delete(int exemplarId, int requisicaoId);
    }
}