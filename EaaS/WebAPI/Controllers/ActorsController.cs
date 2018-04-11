using System;
using System.Threading.Tasks;
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
        [Route("create")]
        public async Task<ActorVariable> PostCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            ActorVariable actorVariable = new ActorVariable();
            actorVariable.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorVariable;
        }

        // Create an actor (Get)
        // Returns the unique actor ID (signed 64-bit integer)
        // ID Type: long
        // ID Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        [HttpGet]
        [Route("create")]
        public async Task<ActorVariable> GetCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            ActorVariable actorVariable = new ActorVariable();
            actorVariable.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            return actorVariable;
        }

        [HttpGet]
        [Route("{id}/{variableName}")]
        public async Task<ActorVariable> GetVariableValue(long id, string variableName)
        {
            ActorId actorId = new ActorId(id);
            ActorVariable actorVariable = new ActorVariable();
            actorVariable.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(variableName);
            actorVariable.VariableValue = variableValue;
            actorVariable.VariableName = variableName;
            return actorVariable;
        }

        [HttpPost]
        [Route("getVariableValue")]
        public async Task<ActorVariable> PostGetVariableValue([FromBody] ActorVariable actorVariable)
        {
            ActorId actorId = new ActorId(actorVariable.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(actorVariable.VariableName);
            actorVariable.VariableValue = variableValue;
            return actorVariable;
        }

        [HttpPost]
        [Route("addVariable")]
        public async Task<ActorVariable> PostAddVariable([FromBody] ActorVariable actorVariable)
        {
            ActorId actorId = new ActorId(actorVariable.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.AddVariableAsync(actorVariable);
            actorVariable.Message = "The new variable [" + actorVariable.VariableName + "] successfully added.";
            return actorVariable;
        }

        [HttpPost]
        [Route("removeVariable")]
        public async Task<ActorVariable> PostRemoveVariable([FromBody] ActorVariable actorVariable)
        {
            ActorId actorId = new ActorId(actorVariable.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.RemoveVariableAsync(actorVariable.VariableName);
            actorVariable.Message = "The variable [" + actorVariable.VariableName + "] successfully removed.";
            return actorVariable;
        }

        [HttpPost]
        [Route("setVariableValue")]
        public async Task<ActorVariable> PostSetVariableValue([FromBody] ActorVariable actorVariable)
        {
            ActorId actorId = new ActorId(actorVariable.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.SetVariableValueAsync(actorVariable.VariableName, actorVariable.VariableValue);
            actorVariable.Message = "The variable [" + actorVariable.VariableName + "] was set to " + actorVariable.VariableValue;
            return actorVariable;
        }

        [HttpPost]
        [Route("execute")]
        public async Task<ActorMethod> PostExecuteMethod([FromBody] ActorMethod actorMethod)
        {
            ActorId actorId = new ActorId(actorMethod.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            ActorMethod response = await actor.ExecuteMethodAsync(actorMethod);
            return response;
        }
    }
}