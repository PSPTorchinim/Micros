using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.App
{
    public abstract class Scope
    {
        public abstract void CreateScope(IServiceCollection services);
    }
}
