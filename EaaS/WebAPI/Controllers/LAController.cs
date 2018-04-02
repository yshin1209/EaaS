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
    public class VecVecMulClass
    {
        public int Id { get; set; }
        public double[] Vector1 { get; set; }
        public double[] Vector2 { get; set; }
    }
 

    public class MatMatMulClass
    {
        public int Id { get; set; }
        public double[][] Matrix1 { get; set; }
        public double[][] Matrix2 { get; set; }
    }

    [Produces("application/json")]
    public class LAController : Controller
    {
        [HttpPost]
        [Route("api/la/blas1")]
        public async Task<double> Blas1([FromBody] VecVecMulClass input)
        {
            int id = input.Id;
            double[] vector1 = input.Vector1;
            double[] vector2 = input.Vector2;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            double output = await actor.VecVecMultiply(vector1, vector2);
            return output;
        }
        [HttpPost]
        [Route("api/la/blas2")]
        public async Task<double[]> Blas2([FromBody] MatVecMulClass input)
        {
            int id = input.Id;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            string json = JsonConvert.SerializeObject(input);
            double[] output = await actor.MatVecMultiply(json);
            return output;
        }

        [HttpPost]
        [Route("api/la/blas3")]
        public async Task<double[][]> Blas3([FromBody] MatMatMulClass input)
        {
            int id = input.Id;
            double[][] matrix1 = input.Matrix1;
            double[][] matrix2 = input.Matrix2;
            ActorId actorId = new ActorId(id);
            var actor = ActorProxy.Create<ILA>(actorId, new Uri("fabric:/Application/LAActorService"));
            double[][] output = await actor.MatMatMultiply(matrix1, matrix2);
            return output;
        }
    }
}