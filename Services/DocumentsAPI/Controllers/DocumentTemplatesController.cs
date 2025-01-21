using DocumentsAPI.Entities;
using DocumentsAPI.Repositories;
using Shared.Services.App;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsAPI.Controllers
{
    public class DocumentTemplatesController : BaseController<DocumentTemplatesController>
    {
        private readonly DocumentTemplatesRepository documentTemplatesRepository;
        public DocumentTemplatesController(ILogger<DocumentTemplatesController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            documentTemplatesRepository = serviceProvider.GetRequiredService<DocumentTemplatesRepository>();
        }

        [HttpGet("GetTemplates")]
        public async Task<ActionResult<List<DocumentTemplate>>> Get()
        {
            var x = await documentTemplatesRepository.Get();
            return Ok(x);
        }

        [HttpGet("GetTemplate")]
        public async Task<ActionResult<List<DocumentTemplate>>> GetByTemplateId([FromQuery]string id)
        {
            var x = await documentTemplatesRepository.Get(x => x.Id == id);
            return Ok(x);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] DocumentTemplate documentTemplate){
            var x = await documentTemplatesRepository.Add(documentTemplate);
            return Ok(x);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Update([FromQuery] string Id, [FromBody] DocumentTemplate documentTemplate){
            
            return NotFound();
        }
    }
}
