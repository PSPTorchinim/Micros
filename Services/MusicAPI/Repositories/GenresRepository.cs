using Shared.Repositories;
using Music.Data;
using Music.Entities;

namespace Music.Repositories
{
    public class GenresRepository : Repository<Genre, MusicContext>, IGenresRepository
    {
        public GenresRepository(MusicContext context, ILogger<IGenresRepository> logger) : base(context, logger)
        {
        }
    }

    public interface IGenresRepository:IRepository<Genre>
    {
    }
}
