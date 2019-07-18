using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository: IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }       

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<Evento> GetAllEventoAsyncById(int eventoid, bool incluirPalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c=> c.RedesSociais);

            if(incluirPalestrantes)
            {
                query = query
                .Include(pe=> pe.PalestrantesEventos)
                .ThenInclude(p=> p.Palestrante);
              }

            query = query.OrderByDescending(c=>c.DataEvento)
            .Where(c=>c.Id == eventoid);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool incluirPalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c=> c.RedesSociais);

            if(incluirPalestrantes)
            {
                query = query
                .Include(pe=> pe.PalestrantesEventos)
                .ThenInclude(p=> p.Palestrante);
              }

            query = query.OrderByDescending(c=>c.DataEvento);

            return await query.ToArrayAsync();

        }


        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool incluirPalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(c => c.Lotes)
            .Include(c=> c.RedesSociais);

            if(incluirPalestrantes)
            {
                query = query
                .Include(pe=> pe.PalestrantesEventos)
                .ThenInclude(p=> p.Palestrante);
              }

            query = query.OrderByDescending(c=>c.DataEvento)
            .Where(c=>c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }


        public async Task<Palestrante> GetAllPalestranteAsync(int palestranteid, bool incluirEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
               .Include(c=> c.RedesSociais);

            if(incluirEventos)
            {
                query = query
                .Include(pe=> pe.PalestranteEventos)
                .ThenInclude(e=> e.Evento);
              }

            query = query.OrderBy(p=>p.Nome)
            .Where(p=>p.Id == palestranteid);         

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string nome, bool incluirEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
               .Include(c=> c.RedesSociais);

            if(incluirEventos)
            {
                query = query
                .Include(pe=> pe.PalestranteEventos)
                .ThenInclude(e=> e.Evento);
              }

            query = query.Where(p=>p.Nome.ToLower().Contains(nome.ToLower()));         

            return await query.ToArrayAsync();
        }

        
    }
}