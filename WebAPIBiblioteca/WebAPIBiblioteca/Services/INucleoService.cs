using WebAPIBiblioteca.DTOs;

namespace WebAPIBiblioteca.Services
{
    public interface INucleoService
    {
        List<NucleoDTO> GetAll();
        NucleoDTO GetById(int id);
        int Create(NucleoCreateDTO dto);
        void Update(int id, NucleoCreateDTO dto);
        void Delete(int id);
    }
}