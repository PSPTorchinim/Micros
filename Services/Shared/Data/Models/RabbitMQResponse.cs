using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data.Models
{
    public class RabbitMQResponse<T> : Response<T> where T : class
    {
        public string RabbitMessage { get; set; }
    }
}
