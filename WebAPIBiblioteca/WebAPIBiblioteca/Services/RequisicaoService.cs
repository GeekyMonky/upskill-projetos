using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class RequisicaoService : IRequisicaoService
    {
        private readonly IRequisicaoRepository _repo;
        private readonly ILogger _logger;

        public RequisicaoService(ILogger<RequisicaoService> logger, IRequisicaoRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<RequisicaoDTO> GetAll()
        {
            _logger.LogInformation("RequisicaoService: GetAll");

            return _repo.GetAll()
                .Select(r => new RequisicaoDTO
                {
                    RequisicaoID = r.RequisicaoID,
                    DataEmprestimo = r.DataEmprestimo,
                    LeitorID = r.LeitorID
                }).ToList();
        }

        public RequisicaoDTO GetById(int id)
        {
            var r = _repo.GetById(id);
            if (r == null) return null;

            return new RequisicaoDTO
            {
                RequisicaoID = r.RequisicaoID,
                DataEmprestimo = r.DataEmprestimo,
                LeitorID = r.LeitorID
            };
        }

        public int Create(RequisicaoCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Requisicao r = new Requisicao { LeitorID = dto.LeitorID };
                int id = _repo.Insert(r, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Delete(int id)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                _repo.Delete(id, trans);
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