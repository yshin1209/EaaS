using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using QuadraticProgramming.Interfaces;
using ClassLibrary;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/QP")]
    public class QPController : Controller
    {
        [HttpPost]
        public async Task<QuadraticProgrammingOutput> PostQP([FromBody] QuadraticProgrammingInput input)
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IQuadraticProgramming>(actorId, new Uri("fabric:/Application/QuadraticProgrammingActorService"));
            string jsonInput = JsonConvert.SerializeObject(input);
            QuadraticProgrammingOutput output = await actor.RunQPAsync(jsonInput);
            return output;
        }
    }
}