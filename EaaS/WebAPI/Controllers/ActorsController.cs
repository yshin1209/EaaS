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
        [HttpGet]
        [Route("{id}/{fieldName}")]
        public async Task<string> GetField(long id, string fieldName)
        {
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var response = await actor.GetFieldAsync(fieldName);
            return response;
        }


        [HttpPost]
        public async Task<ActorId> PostActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorId;
        }

        [HttpPost]
        [Route("addField")]
        public async Task<string> PostAddField([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.AddFieldAsync(actorData.fieldName);
            return ("The new field " + actorData.fieldName + " was added");
        }

        [HttpPost]
        [Route("removeField")]
        public async Task<string> PostRemoveField([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.RemoveFieldAsync(actorData.fieldName);
            return ("The field " + actorData.fieldName + " was removed");
        }

        [HttpPost]
        [Route("setField")]
        public async Task<string> PostSetField([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.SetFieldAsync(actorData.fieldName, actorData.fieldValue);
            return ("The field " + actorData.fieldName + " was set to " + actorData.fieldValue);
        }
    }

    public class ActorData
    {
        public long id { get; set; }
        public string fieldName { get; set; }
        public string fieldValue { get; set; }
    }
}