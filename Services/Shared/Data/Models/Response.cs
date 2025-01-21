using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Data.Models
{
    public class Response<T>
    {
        public bool Error { get; set; }
        public T ResponseObject { get; set; }
        public String ErrorMessage { get; set; }
    }
}
