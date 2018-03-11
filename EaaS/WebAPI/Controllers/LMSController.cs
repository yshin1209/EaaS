using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using LMS.Interfaces;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/LMS")]
    public class LMSController : Controller
    {
        [HttpGet]
        [Route("{id}/{timeDelay}")]
        public async Task<int> Get(int id, int timeDelay)
        {
            ActorId actorId = new ActorId(id);
            var actor1 = ActorProxy.Create<ILMS>(actorId, new Uri("fabric:/Application/LMSActorService"));
            await actor1.StartLMSAsync(timeDelay);
            return 0;
        }

        [HttpGet]
        [Route("{id}/{stepSize}/{inputValue}/{outputValue}")]
        public async Task<double[]> Get(int id, double stepSize, double inputValue, double outputValue)
        {
            ActorId actorId = new ActorId(id);
            var actor1 = ActorProxy.Create<ILMS>(actorId, new Uri("fabric:/Application/LMSActorService"));
            var response = await actor1.RunLMSAsync(stepSize, inputValue, outputValue);
            return response;
        }
    }
}