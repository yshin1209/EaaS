using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ActorData
    {
        public long Id { get; set; }
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public string Message { get; set; }
    }
}
