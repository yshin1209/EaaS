using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class HttpMethodInput
    {
        public string Uri { get; set; }
        public HttpMethodBody Body { get; set; }
    }

    public class HttpMethodBody
    {
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
    }
}
