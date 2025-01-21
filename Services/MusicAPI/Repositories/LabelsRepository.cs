using Shared.Repositories;
using Music.Data;
using Music.Entities;

namespace Music.Repositories
{
    public class LabelsRepository : Repository<Label, MusicContext>, ILabelsRepository
    {
        public LabelsRepository(MusicContext context, ILogger<ILabelsRepository> logger) : base(context, logger)
        {
        }
    }

    public interface ILabelsRepository:IRepository<Label>
    {
    }
}
