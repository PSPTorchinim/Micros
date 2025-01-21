using Shared.Services.App;

namespace DocumentsAPI.Controllers
{
    public class DocumentsController : BaseController<DocumentsController>
    {
        public DocumentsController(ILogger<DocumentsController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
        }
    }
}
