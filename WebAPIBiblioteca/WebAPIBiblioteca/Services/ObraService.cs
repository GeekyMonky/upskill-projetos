using LibDB;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Models;
using WebAPIBiblioteca.Repositories;

namespace WebAPIBiblioteca.Services
{
    public class ObraService : IObraService
    {
        private readonly IObraRepository _repo;
        private readonly ILogger _logger;

        public ObraService(ILogger<ObraService> logger, IObraRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public List<ObraDTO> GetAll()
        {
            _logger.LogInformation("ObraService: GetAll");

            return _repo.GetAll()
                .Select(o => new ObraDTO
                {
                    ObraID = o.ObraID,
                    Titulo = o.Titulo,
                    Autor = o.Autor,
                    ISBN = o.ISBN,
                    Capa = o.Capa,
                    TemaID = o.TemaID
                }).ToList();
        }

        public ObraDTO GetById(int id)
        {
            var o = _repo.GetById(id);
            if (o == null) return null;

            return new ObraDTO
            {
                ObraID = o.ObraID,
                Titulo = o.Titulo,
                Autor = o.Autor,
                ISBN = o.ISBN,
                Capa = o.Capa,
                TemaID = o.TemaID
            };
        }

        public int Create(ObraCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Obra o = new Obra
                {
                    Titulo = dto.Titulo,
                    Autor = dto.Autor,
                    ISBN = dto.ISBN,
                    Capa = dto.Capa,
                    TemaID = dto.TemaID
                };
                int id = _repo.Insert(o, trans);
                DALPro.Commit(trans);
                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, ObraCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();
            try
            {
                Obra o = new Obra
                {
                    ObraID = id,
                    Titulo = dto.Titulo,
                    Autor = dto.Autor,
                    ISBN = dto.ISBN,
                    Capa = dto.Capa,
                    TemaID = dto.TemaID
                };
                _repo.Update(o, trans);
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