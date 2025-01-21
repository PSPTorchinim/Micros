using Shared.Repositories;
using Music.Data;
using Music.Entities;

namespace Music.Repositories
{
    public class TagsRepository : Repository<Tag, MusicContext>, ITagsRepository
    {
        public TagsRepository(MusicContext context, ILogger<ITagsRepository> logger) : base(context, logger)
        {
        }
    }

    public interface ITagsRepository:IRepository<Tag>
    {
    }
}
