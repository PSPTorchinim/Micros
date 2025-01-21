using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyAPI.Services;
using Shared.Services.App;

namespace CompanyAPI.Controlles
{
    public class PackagesController : BaseController<PackagesController>
    {
        private readonly IPackagesService PackagesService;
        public PackagesController(ILogger<PackagesController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            PackagesService = serviceProvider.GetRequiredService<IPackagesService>();
        }
    }
}