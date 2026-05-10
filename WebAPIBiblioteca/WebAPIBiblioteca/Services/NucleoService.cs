using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class NucleoService : INucleoService
    {
        private readonly INucleoRepository _repo;
        private readonly ILogger _logger;

        public NucleoService(ILogger<NucleoService> logger, INucleoRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<NucleoDTO> GetAll()
        {
            _logger.LogInformation("NucleoService: GetAll");

            return _repo.GetAll()
                .Select(n => new NucleoDTO
                {
                    NucleoID = n.NucleoID,
                    Distrito = n.Distrito,
                    EhCentral = n.EhCentral
                }).ToList();
        }

        public NucleoDTO GetById(int id)
        {
            var n = _repo.GetById(id);
            if (n == null) return null;

            return new NucleoDTO
            {
                NucleoID = n.NucleoID,
                Distrito = n.Distrito,
                EhCentral = n.EhCentral
            };
        }

        public int Create(NucleoCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Nucleo n = new Nucleo { Distrito = dto.Distrito, EhCentral = dto.EhCentral };
                int id = _repo.Insert(n, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, NucleoCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Nucleo n = new Nucleo
                {
                    NucleoID = id,
                    Distrito = dto.Distrito,
                    EhCentral = dto.EhCentral
                };
                _repo.Update(n, trans);
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