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
using Newtonsoft.Json;
using RestSharp;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public async Task<string> PostCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            ActorData actorData = new ActorData();
            actorData.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        // Create an actor (Get)
        // Returns the unique actor ID (signed 64-bit integer)
        // ID Type: long
        // ID Range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        [HttpGet]
        [Route("create")]
        public async Task<string> GetCreateActor()
        {
            ActorId actorId = ActorId.CreateRandom();
            ActorData actorData = new ActorData();
            actorData.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.CreateActorAsync();
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        [HttpGet]
        [Route("{id}/{variableName}")]
        public async Task<string> GetVariableValue(long id, string variableName)
        {
            ActorId actorId = new ActorId(id);
            ActorData actorData = new ActorData();
            actorData.Id = actorId.GetLongId();
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(variableName);
            actorData.VariableValue = variableValue;
            actorData.VariableName = variableName;
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        [HttpPost]
        [Route("getVariableValue")]
        public async Task<string> PostGetVariableValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            var variableValue = await actor.GetVariableValueAsync(actorData.VariableName);
            actorData.VariableValue = variableValue;
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        [HttpPost]
        [Route("addVariable")]
        public async Task<string> PostAddVariable([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.AddVariableAsync(actorData);
            actorData.Message = "The new variable [" + actorData.VariableName + "] successfully added.";
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        [HttpPost]
        [Route("removeVariable")]
        public async Task<string> PostRemoveVariable([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.RemoveVariableAsync(actorData.VariableName);
            actorData.Message = "The variable [" + actorData.VariableName + "] successfully removed.";
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        [HttpPost]
        [Route("setVariableValue")]
        public async Task<string> PostSetVariableValue([FromBody] ActorData actorData)
        {
            ActorId actorId = new ActorId(actorData.Id);
            var actor = ActorProxy.Create<IActors>(actorId, new Uri("fabric:/Application/ActorsActorService"));
            await actor.SetVariableValueAsync(actorData.VariableName, actorData.VariableValue);
            actorData.Message = "The variable [" + actorData.VariableName + "] was set to " + actorData.VariableValue;
            string jsonActorData = JsonConvert.SerializeObject(actorData);
            return jsonActorData;
        }

        /*[HttpPost]
        [Consumes("application/json")]
        [Route("executeMethod")]
        public async string PostExecuteMethod([FromBody] HttpMethodInput input)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(input.Uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            HttpResponseMessage response = await client.PostAsync(
     "", input.Body);
            response.EnsureSuccessStatusCode();

            string responseContent = "";
            string httpMethodUri = input.Uri;

            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest(httpMethodUri, Method.POST);
            request.AddParameter("application/json", input, ParameterType.RequestBody);
            client.ExecuteAsync(request, response => {
                responseContent = response.Content;
            });
            return responseContent;
        }*/
    }
}