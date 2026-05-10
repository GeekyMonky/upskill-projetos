using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface IExemplarService
    {
        List<ExemplarDTO> GetAll();
        ExemplarDTO GetById(int id);
        int Create(ExemplarCreateDTO dto);
        void Update(int id, ExemplarCreateDTO dto);
        void Delete(int id);
    }
}