using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ActorMethod
    {
        public long Id { get; set; }
        public string HttpMethod { get; set; }
        public string MethodUrl { get; set; }
        public string Authorization { get; set; }
        public string Headers{ get; set; }
        public string Body { get; set; }
        public string Response { get; set; }
        public string Message { get; set; }
    }
}
