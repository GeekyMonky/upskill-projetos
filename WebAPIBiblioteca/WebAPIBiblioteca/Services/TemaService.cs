using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class TemaService : ITemaService
    {
        private readonly ITemaRepository _repo;
        private readonly ILogger _logger;

        public TemaService(ILogger<TemaService> logger, ITemaRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<TemaDTO> GetAll()
        {
            _logger.LogInformation("TemaService: GetAll");

            return _repo.GetAll()
                .Select(t => new TemaDTO
                {
                    TemaID = t.TemaID,
                    Designacao = t.Designacao
                }).ToList();
        }

        public TemaDTO GetById(int id)
        {
            var t = _repo.GetById(id);
            if (t == null) return null;

            return new TemaDTO
            {
                TemaID = t.TemaID,
                Designacao = t.Designacao
            };
        }

        public int Create(TemaCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Tema t = new Tema { Designacao = dto.Designacao };
                int id = _repo.Insert(t, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, TemaCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Tema t = new Tema { TemaID = id, Designacao = dto.Designacao };
                _repo.Update(t, trans);
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