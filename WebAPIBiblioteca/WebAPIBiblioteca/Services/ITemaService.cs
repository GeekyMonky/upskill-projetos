using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface ITemaService
    {
        List<TemaDTO> GetAll();
        TemaDTO GetById(int id);
        int Create(TemaCreateDTO dto);
        void Update(int id, TemaCreateDTO dto);
        void Delete(int id);
    }
}