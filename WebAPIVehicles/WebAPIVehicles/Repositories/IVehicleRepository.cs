using Microsoft.Data.SqlClient;
using WebAPIVehicles.Models;

namespace WebAPIVehicles.Repositories
{
    public interface IVehicleRepository
    {
        List<Vehicle> GetAll();

        Vehicle GetById(int id);

        int Insert(Vehicle vehicle, SqlTransaction trans);

        void Update(Vehicle vehicle, SqlTransaction trans);

        void Delete(int id, SqlTransaction trans);
    }
}