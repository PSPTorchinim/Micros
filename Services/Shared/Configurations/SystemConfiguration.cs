using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class SystemConfiguration
    {
        public string ConnectionString { get; set; }
        public TokenConfiguration TokenConfiguration { get; set; }
        public string RabbitMQConnection { get; set; }
    }
}
