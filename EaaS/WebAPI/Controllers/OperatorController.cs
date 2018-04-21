using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using ClassLibrary;
using Microsoft.ServiceFabric.Actors.Client;
using Operator.Interfaces;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Operator")]
    public class OperatorController : Controller
    {
        [HttpPost]
        [Route("greaterThan")]
        public async Task<string> PostGreaterThan([FromBody] HttpMethodInput input)
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IOperator>(actorId, new Uri("fabric:/Application/OperatorActorService"));
            bool response = await actor.GreaterThanAsync(input);
            string jsonResponse = JsonConvert.SerializeObject(response);
            return jsonResponse;
        }
    }
}