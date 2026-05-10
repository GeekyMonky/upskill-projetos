using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class ExemplarService : IExemplarService
    {
        private readonly IExemplarRepository _repo;
        private readonly ILogger _logger;

        public ExemplarService(ILogger<ExemplarService> logger, IExemplarRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<ExemplarDTO> GetAll()
        {
            _logger.LogInformation("ExemplarService: GetAll");

            return _repo.GetAll()
                .Select(e => new ExemplarDTO
                {
                    ExemplarID = e.ExemplarID,
                    ObraID = e.ObraID,
                    NucleoID = e.NucleoID
                }).ToList();
        }

        public ExemplarDTO GetById(int id)
        {
            var e = _repo.GetById(id);
            if (e == null) return null;

            return new ExemplarDTO
            {
                ExemplarID = e.ExemplarID,
                ObraID = e.ObraID,
                NucleoID = e.NucleoID
            };
        }

        public int Create(ExemplarCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Exemplar e = new Exemplar
                {
                    ObraID = dto.ObraID,
                    NucleoID = dto.NucleoID
                };
                int id = _repo.Insert(e, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, ExemplarCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Exemplar e = new Exemplar
                {
                    ExemplarID = id,
                    ObraID = dto.ObraID,
                    NucleoID = dto.NucleoID
                };
                _repo.Update(e, trans);
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