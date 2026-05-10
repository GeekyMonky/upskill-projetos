using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class LeitorService : ILeitorService
    {
        private readonly ILeitorRepository _repo;
        private readonly ILogger _logger;

        public LeitorService(ILogger<LeitorService> logger, ILeitorRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<LeitorDTO> GetAll()
        {
            _logger.LogInformation("LeitorService: GetAll");

            return _repo.GetAll()
                .Select(l => new LeitorDTO
                {
                    LeitorID = l.LeitorID,
                    Nome = l.Nome,
                    Email = l.Email,
                    EstaAtivo = l.EstaAtivo,
                    NumeroAtrasos = l.NumeroAtrasos,
                    DataUltimaRequisicao = l.DataUltimaRequisicao,
                    DataRegisto = l.DataRegisto
                }).ToList();
        }

        public LeitorDTO GetById(int id)
        {
            var l = _repo.GetById(id);
            if (l == null) return null;

            return new LeitorDTO
            {
                LeitorID = l.LeitorID,
                Nome = l.Nome,
                Email = l.Email,
                EstaAtivo = l.EstaAtivo,
                NumeroAtrasos = l.NumeroAtrasos,
                DataUltimaRequisicao = l.DataUltimaRequisicao,
                DataRegisto = l.DataRegisto
            };
        }

        public int Create(LeitorCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Leitor l = new Leitor
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    EstaAtivo = dto.EstaAtivo
                };
                int id = _repo.Insert(l, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, LeitorCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Leitor l = new Leitor
                {
                    LeitorID = id,
                    Nome = dto.Nome,
                    Email = dto.Email,
                    EstaAtivo = dto.EstaAtivo
                };
                _repo.Update(l, trans);
                DALPro.Commit(trans);
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