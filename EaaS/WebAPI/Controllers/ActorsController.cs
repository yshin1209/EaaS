using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Actors.Interfaces;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Actors")]
    public class ActorsController : Controller
    {
        [HttpPost]
        public async Task<ActorId> Post([FromBody] ActorData actorData)
        {
            string actorName = actorData.actorName;
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync(actorName);
            return actorId;
        }
    }

    public class ActorData
    {
        public string actorName { get; set; }
    }
}