using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class ExemplarRequisicaoService : IExemplarRequisicaoService
    {
        private readonly IExemplarRequisicaoRepository _repo;
        private readonly ILogger _logger;

        public ExemplarRequisicaoService(ILogger<ExemplarRequisicaoService> logger, IExemplarRequisicaoRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<ExemplarRequisicaoDTO> GetByRequisicao(int requisicaoId)
        {
            _logger.LogInformation($"ExemplarRequisicaoService: GetByRequisicao({requisicaoId})");

            return _repo.GetByRequisicao(requisicaoId)
                .Select(er => new ExemplarRequisicaoDTO
                {
                    ExemplarID = er.ExemplarID,
                    RequisicaoID = er.RequisicaoID,
                    DataDevolucao = er.DataDevolucao
                }).ToList();
        }

        public List<ExemplarRequisicaoDTO> GetByExemplar(int exemplarId)
        {
            _logger.LogInformation($"ExemplarRequisicaoService: GetByExemplar({exemplarId})");

            return _repo.GetByExemplar(exemplarId)
                .Select(er => new ExemplarRequisicaoDTO
                {
                    ExemplarID = er.ExemplarID,
                    RequisicaoID = er.RequisicaoID,
                    DataDevolucao = er.DataDevolucao
                }).ToList();
        }

        public ExemplarRequisicaoDTO GetByPair(int exemplarId, int requisicaoId)
        {
            var er = _repo.GetByPair(exemplarId, requisicaoId);
            if (er == null) return null;

            return new ExemplarRequisicaoDTO
            {
                ExemplarID = er.ExemplarID,
                RequisicaoID = er.RequisicaoID,
                DataDevolucao = er.DataDevolucao
            };
        }

        public void Adicionar(int requisicaoId, ExemplarRequisicaoCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                ExemplarRequisicao er = new ExemplarRequisicao
                {
                    ExemplarID = dto.ExemplarID,
                    RequisicaoID = requisicaoId,
                    DataDevolucao = null
                };
                _repo.Insert(er, trans);
                DALPro.Commit(trans);
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void MarcarDevolvido(int exemplarId, int requisicaoId)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                _repo.MarcarDevolvido(exemplarId, requisicaoId, trans);
                DALPro.Commit(trans);
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Delete(int exemplarId, int requisicaoId)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                _repo.Delete(exemplarId, requisicaoId, trans);
                DALPro.Commit(trans);
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }
    }
}