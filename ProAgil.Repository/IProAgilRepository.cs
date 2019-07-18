
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
      void Add<T>(T entity) where T : class; 
      void Update<T>(T entity) where T : class; 
      void Delete<T>(T entity) where T : class; 

      Task<bool> SaveChangesAsync();

      Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool incluirPalestrantes);
      Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes);
      Task<Evento> GetAllEventoAsyncById(int eventoid, bool incluirPalestrantes);

      Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string nome, bool incluirEventos);
      Task<Palestrante> GetAllPalestranteAsync(int palestranteid, bool incluirEventos);

    }
}