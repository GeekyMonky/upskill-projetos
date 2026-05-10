using LibDB;
using WebAPIVehicles.DTOs;
using WebAPIVehicles.Models;
using WebAPIVehicles.Repositories;

namespace WebAPIVehicles.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repo;

        public VehicleService(IVehicleRepository repo)
        {
            _repo = repo;
        }

        public List<VehicleDTO> GetAll()
        {
            return _repo.GetAll()
                .Select(v => new VehicleDTO
                {
                    VehicleID = v.VehicleID,
                    Brand = v.Brand,
                    Model = v.Model,
                    Year = v.Year,
                    LastInspection = v.LastInspection,
                    Sold = v.Sold
                }).ToList();
        }

        public VehicleDTO GetById(int id)
        {
            var v = _repo.GetById(id);

            if (v == null)
                return null;

            return new VehicleDTO
            {
                VehicleID = v.VehicleID,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                LastInspection = v.LastInspection,
                Sold = v.Sold
            };
        }

        public int Create(VehicleCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();

            try
            {
                Vehicle v = new Vehicle
                {
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    LastInspection = dto.LastInspection,
                    Sold = dto.Sold
                };

                int id = _repo.Insert(v, trans);

                DALPro.Commit(trans);

                return id;
            }
            catch
            {
                DALPro.Rollback(trans);
                throw;
            }
        }

        public void Update(int id, VehicleCreateDTO dto)
        {
            var trans = DALPro.BeginTransaction();

            try
            {
                Vehicle v = new Vehicle
                {
                    VehicleID = id,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    LastInspection = dto.LastInspection,
                    Sold = dto.Sold
                };

                _repo.Update(v, trans);

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