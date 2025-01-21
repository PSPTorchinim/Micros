using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Database
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
