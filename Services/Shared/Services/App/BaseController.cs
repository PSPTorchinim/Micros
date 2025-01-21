using Shared.Data.Exceptions;
using Shared.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shared.Services.App
{
    [EnableCors("cors")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController<TController> : ControllerBase where TController : BaseController<TController>
    {
        private readonly ILogger<TController> _logger;
        public readonly IServiceProvider _serviceProvider;

        public BaseController(ILogger<TController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [HttpGet("Hello")]
        [AllowAnonymous]
        public async Task<IActionResult> Hello()
        {
            return Ok("Hello");
        }

        public async Task<IActionResult> Handle<T>(Func<Task<T>> action)
        {
            var response = new Response<T>();
            try
            {
                response.ResponseObject = await ExceptionHandler.Handle(action, _logger);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorMessage = ex.Message;
                ExceptionHandler.LogException(ex, _logger);
                return BadRequest(response);
            }
        }
    }
}
