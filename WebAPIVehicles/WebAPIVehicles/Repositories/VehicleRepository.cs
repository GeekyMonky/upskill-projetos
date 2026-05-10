using LibDB;
using Microsoft.Data.SqlClient;
using WebAPIVehicles.Models;

namespace WebAPIVehicles.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public List<Vehicle> GetAll()
        {
            string sql = "SELECT * FROM Vehicles";
            return DALPro.Query<Vehicle>(sql);
        }

        public Vehicle GetById(int id)
        {
            string sql = "SELECT * FROM Vehicles WHERE VehicleID=@id";

            var param = new Dictionary<string, object>
            {
                {"@id", id}
            };

            return DALPro.Query<Vehicle>(sql, param).FirstOrDefault();
        }

        public int Insert(Vehicle v, SqlTransaction trans)
        {
            string sql = @"INSERT INTO Vehicles
                           (Brand, Model, Year, LastInspection, Sold)
                           VALUES
                           (@Brand, @Model, @Year, @LastInspection, @Sold);

                           SELECT SCOPE_IDENTITY();";

            var param = new Dictionary<string, object>
            {
                {"@Brand", v.Brand},
                {"@Model", v.Model},
                {"@Year", v.Year},
                {"@LastInspection", (object)v.LastInspection ?? DBNull.Value},
                {"@Sold", v.Sold}
            };

            return Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
        }

        public void Update(Vehicle v, SqlTransaction trans)
        {
            string sql = @"UPDATE Vehicles
                           SET Brand=@Brand,
                               Model=@Model,
                               Year=@Year,
                               LastInspection=@LastInspection,
                               Sold=@Sold
                           WHERE VehicleID=@VehicleID";

            var param = new Dictionary<string, object>
            {
                {"@VehicleID", v.VehicleID},
                {"@Brand", v.Brand},
                {"@Model", v.Model},
                {"@Year", v.Year},
                {"@LastInspection", (object)v.LastInspection ?? DBNull.Value},
                {"@Sold", v.Sold}
            };

            DALPro.Execute(sql, param, trans);
        }

        public void Delete(int id, SqlTransaction trans)
        {
            string sql = "DELETE FROM Vehicles WHERE VehicleID=@id";

            var param = new Dictionary<string, object>
            {
                {"@id", id}
            };

            DALPro.Execute(sql, param, trans);
        }
    }
}