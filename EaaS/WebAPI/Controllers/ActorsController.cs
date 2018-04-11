using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Actors.Interfaces;
using ClassLibrary;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/actors")]
    public class ActorsController : Controller
    {

        /// <summary>
        /// Create an actor (POST)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/actors
        ///     {
        ///     }
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created unique actor ID (signed 64-bit integer)</returns>
        /// <returns> ID Type: long </returns>
        /// <returns> ID Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807 </returns>
        /// <response code="201">Returns the newly created actor ID</response>

        [HttpPost]
        [ProducesResponseType(201)]
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
        [Route("{id}/{variableName}")]
        public async Task<string> GetVariableValue(long id, string variableName)
        {
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(variableName);
            return variableValue;
        }

        [HttpPost]
        [Route("getVariableValue")]
        public async Task<string> PostGetVariableValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(actorData.VariableName);
            return (variableValue);
        }

        [HttpPost]
        [Route("addVariable")]
        public async Task<string> PostAddVariable([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.AddVariableAsync(actorData);
            return ("The new field " + actorData.VariableName + " was added");
        }

        [HttpPost]
        [Route("removeVariable")]
        public async Task<string> PostRemoveVariable([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.RemoveVariableAsync(actorData.VariableName);
            return ("The field " + actorData.VariableName + " was removed");
        }

        [HttpPost]
        [Route("setVariableValue")]
        public async Task<string> PostSetVariableValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.SetVariableValueAsync(actorData.VariableName, actorData.VariableValue);
            return ("The field " + actorData.VariableName + " was set to " + actorData.VariableValue);
        }

        [HttpPost]
        [Route("execute")]
        public async Task<ActorId> PostExecuteMethod()
        {
            ActorId actorId = ActorId.CreateRandom();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorId;
        }
    }
}