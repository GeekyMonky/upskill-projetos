using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface IObraService
    {
        List<ObraDTO> GetAll();
        ObraDTO GetById(int id);
        int Create(ObraCreateDTO dto);
        void Update(int id, ObraCreateDTO dto);
        void Delete(int id);
    }
}