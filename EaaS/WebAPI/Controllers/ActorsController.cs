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
    [Route("api/actors")]
    public class ActorsController : Controller
    {
        // Create an actor (POST)
        // Returns the unique actor ID (signed 64-bit integer)
        // ID Type: long
        // ID Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        [HttpPost]
        [Route("create")]
        public async Task<ActorId> PostCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorId;
        }

        // Create an actor (Get)
        // Returns the unique actor ID (signed 64-bit integer)
        // ID Type: long
        // ID Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        [HttpGet]
        [Route("create")]
        public async Task<ActorId> GetCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorId;
        }

        [HttpGet]
        [Route("{id}/{fieldName}")]
        public async Task<string> GetFieldValue(long id, string fieldName)
        {
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var fieldValue = await actor.GetFieldValueAsync(fieldName);
            return fieldValue;
        }

        [HttpPost]
        [Route("getFieldValue")]
        public async Task<string> PostGetFieldValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var response = await actor.GetFieldValueAsync(actorData.fieldName);
            return (response);
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
        [Route("setFieldValue")]
        public async Task<string> PostSetFieldValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.SetFieldValueAsync(actorData.fieldName, actorData.fieldValue);
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