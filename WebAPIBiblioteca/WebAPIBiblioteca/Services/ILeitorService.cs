using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface ILeitorService
    {
        List<LeitorDTO> GetAll();
        LeitorDTO GetById(int id);
        int Create(LeitorCreateDTO dto);
        void Update(int id, LeitorCreateDTO dto);
        void Delete(int id);
    }
}