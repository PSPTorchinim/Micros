using Shared.Repositories;
using Music.Data;
using Music.Entities;

namespace Music.Repositories
{
    public class PlaylistsRepository : Repository<Playlist, MusicContext>, IPlaylistsRepository
    {
        public PlaylistsRepository(MusicContext context, ILogger<IPlaylistsRepository> logger) : base(context, logger)
        {
        }
    }

    public interface IPlaylistsRepository:IRepository<Playlist>
    {
    }
}
