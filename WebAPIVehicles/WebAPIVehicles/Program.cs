using LibDB;
using WebAPIVehicles.DTOs;
using WebAPIVehicles.Repositories;
using WebAPIVehicles.Services;

namespace WebAPIVehicles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DALPro.ConnectionString = builder.Configuration.GetConnectionString("Vehicles");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "WebAPI Vehicles");

            app.MapGet("/vehicles", (IVehicleService service) =>
            {
                return service.GetAll();
            });

            app.MapGet("/vehicles/{id}", (int id, IVehicleService service) =>
            {
                var v = service.GetById(id);
                return v == null ? Results.NotFound() : Results.Ok(v);
            });

            app.MapPost("/vehicles", (VehicleCreateDTO dto, IVehicleService service) =>
            {
                int id = service.Create(dto);
                return Results.Created($"/vehicles/{id}", new { id });
            });

            app.MapPut("/vehicles/{id}", (int id, VehicleCreateDTO dto, IVehicleService service) =>
            {
                service.Update(id, dto);
                return Results.Ok();
            });

            app.MapDelete("/vehicles/{id}", (int id, IVehicleService service) =>
            {
                service.Delete(id);
                return Results.Ok();
            });

            app.Run();
        }
    }
}