using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Services;
using Shared.Services.App;
using Microsoft.AspNetCore.Mvc;
using CompanyAPI.Data.Models;

namespace CompanyAPI.Controlles
{
    public class BrandsController : BaseController<BrandsController>
    {
        private readonly IBrandsService BrandsService;
        public BrandsController(ILogger<BrandsController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            BrandsService = serviceProvider.GetRequiredService<IBrandsService>();
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetBrands()
        {
            return Ok(await BrandsService.Get());
        }

        [HttpPost]
        public async Task<ActionResult<object>> RegisterBrand(RegisterBrandDTO register){
            return Ok(await BrandsService.RegisterBrand(register));
        }
    }
}