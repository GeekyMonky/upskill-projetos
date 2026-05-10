using WebAPIVehicles.DTOs;

namespace WebAPIVehicles.Services
{
    public interface IVehicleService
    {
        List<VehicleDTO> GetAll();

        VehicleDTO GetById(int id);

        int Create(VehicleCreateDTO dto);

        void Update(int id, VehicleCreateDTO dto);

        void Delete(int id);
    }
}