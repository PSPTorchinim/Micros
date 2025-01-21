using Shared.Repositories;
using Music.Data;

namespace Music.Repositories
{
    public class DirectoriesRepository : Repository<Entities.Directory, MusicContext>, IDirectoriesRepository
    {
        public DirectoriesRepository(MusicContext context, ILogger<IDirectoriesRepository> logger) : base(context, logger)
        {
        }
    }

    public interface IDirectoriesRepository:IRepository<Entities.Directory>
    {
    }
}
