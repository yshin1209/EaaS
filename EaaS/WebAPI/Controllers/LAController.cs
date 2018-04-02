using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using LA.Interfaces;
using Newtonsoft.Json;
using ClassLibrary;

namespace WebAPI.Controllers
{

    [Produces("application/json")]
    public class LAController : Controller
    {
        [HttpPost]
        [Route("api/la/blas1")]
        public async Task<double> Blas1([FromBody] VecVecMulClass input)
        {
            int id = input.Id;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            string jsonInput = JsonConvert.SerializeObject(input);
            double output = await actor.VecVecMultiply(jsonInput);
            return output;
        }
        [HttpPost]
        [Route("api/la/blas2")]
        public async Task<double[]> Blas2([FromBody] MatVecMulClass input)
        {
            int id = input.Id;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            string jsonInput = JsonConvert.SerializeObject(input);
            double[] output = await actor.MatVecMultiply(jsonInput);
            return output;
        }

        [HttpPost]
        [Route("api/la/blas3")]
        public async Task<string> Blas3([FromBody] MatMatMulClass input)
        {
            int id = input.Id;
            double[,] matrix1 = input.Matrix1;
            double[,] matrix2 = input.Matrix2;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            string jsonInput = JsonConvert.SerializeObject(input);
            string output = await actor.MatMatMultiply(jsonInput);
            return output;
        }
    }
}